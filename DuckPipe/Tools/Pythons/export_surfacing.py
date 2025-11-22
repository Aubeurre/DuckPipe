# -*- coding: utf-8 -*-
"""
Cross-DCC surfacing exporter (Maya & Blender)

What it does:
- Finds materials whose name ends with "_MAT"
- For each material, collects full node graph (all upstream nodes)
- Lists all meshes/transforms assigned to that material (full DAG paths)
- Dumps a JSON with enough data to reconstruct the shader graph and assignations.

"""

import json
import os
import sys
import mathutils
from collections import deque

# detect environment
IN_BLENDER = False
IN_MAYA = False
try:
    import bpy
    IN_BLENDER = True
except Exception:
    pass

try:
    import maya.cmds as cmds
    IN_MAYA = True
except Exception:
    pass


# ----------------------------
# Four tout 
# ----------------------------
def write_json(data, out_path):
    os.makedirs(os.path.dirname(out_path), exist_ok=True)
    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    print(f"[export_surfacing] Wrote JSON -> {out_path}")


# ----------------------------
# Maya
# ----------------------------
if IN_MAYA:
    def maya_list_materials():
        mats = cmds.ls(materials=True) or []
        mats = [m for m in mats if m.endswith("_MAT")]
        return mats

    def maya_get_assigned_meshes(mat):
        # find shading engines connected to the material
        sgs = cmds.listConnections(mat, type="shadingEngine") or []
        meshes = []
        for sg in sgs:
            try:
                members = cmds.sets(sg, q=True) or []
            except Exception:
                members = []
            # keep only transforms (full path)
            for m in members:
                # filter shapes -> get transform
                if cmds.objExists(m):
                    # if it's a shape, get parent transform
                    typ = cmds.nodeType(m)
                    if typ.endswith("Shape"):
                        parent = cmds.listRelatives(m, p=True, f=True) or []
                        if parent:
                            meshes.append(parent[0])
                    else:
                        # could be transform already
                        meshes.append(m)
        # unique and fullPath
        return sorted(list(dict.fromkeys(meshes)))

    def maya_collect_graph(start_node):
        """
        BFS upstream from start_node, collect nodes, attrs, connections.
        Returns dict: nodes{node:{type, attrs}}, connections: list of (srcNode, srcAttr, dstNode, dstAttr)
        """
        nodes = {}
        connections = []

        queue = deque([start_node])
        seen = set()

        while queue:
            node = queue.popleft()
            if node in seen:
                continue
            seen.add(node)

            try:
                ntype = cmds.nodeType(node)
            except Exception:
                ntype = "unknown"

            node_info = {"type": ntype, "attributes": {}}
            # collect attributes (listAttr returns many; we'll try to get readable ones)
            attrs = cmds.listAttr(node) or []
            for a in attrs:
                # skip hidden internal attrs that error often
                # try to read value
                full_attr = f"{node}.{a}"
                try:
                    # Get connection info first
                    src = cmds.listConnections(full_attr, s=True, d=False, p=True) or []
                    dst = cmds.listConnections(full_attr, s=False, d=True, p=True) or []
                    # record connections (we will walk connections separately)
                    for splug in src:
                        # splug format: sourceNode.attr
                        parts = splug.split(".")
                        src_node = parts[0]
                        src_attr = ".".join(parts[1:])
                        connections.append((src_node, src_attr, node, a))
                        # enqueue source node
                        if src_node not in seen:
                            queue.append(src_node)

                    # only store scalar values and strings; ignore huge arrays
                    val = None
                    try:
                        val = cmds.getAttr(full_attr)
                    except Exception:
                        # try generic getAttr with asString
                        try:
                            val = cmds.getAttr(full_attr, asString=True)
                        except Exception:
                            val = None

                    # store if not None
                    if val is not None:
                        # convert Maya objects to python types (e.g. MObjects->str)
                        node_info["attributes"][a] = val
                except Exception:
                    # ignore problematic attrs
                    continue

            nodes[node] = node_info

            # enqueue upstream connected nodes found by listConnections (safer to find nodes that feed this node)
            upstream = cmds.listConnections(node, s=True, d=False, p=False) or []
            for up in upstream:
                if up not in seen:
                    queue.append(up)

        # deduplicate connections
        conn_unique = []
        seen_conn = set()
        for c in connections:
            key = tuple(c)
            if key not in seen_conn:
                conn_unique.append({"src_node": c[0], "src_attr": c[1], "dst_node": c[2], "dst_attr": c[3]})
                seen_conn.add(key)

        return {"nodes": nodes, "connections": conn_unique}

    def maya_build_shader_entry(mat):
        entry = {"name": mat, "engine": "maya", "graph": None, "assignments": []}
        # try to get surface shader node (mat itself might be shader)
        start_node = mat
        # collect mesh assignments
        entry["assignments"] = maya_get_assigned_meshes(mat)
        # collect full graph upstream of the shader node
        graph = maya_collect_graph(start_node)
        entry["graph"] = graph
        return entry


# ----------------------------
# Blender
# ----------------------------
if IN_BLENDER:
    def safe_to_python(value):
        if value is None:
            return None
        if isinstance(value, (int, float, str, bool)):
            return value
        if isinstance(value, (list, tuple, set)):
            return [safe_to_python(v) for v in value]
        if isinstance(value, mathutils.Vector):
            return list(value)
        if isinstance(value, mathutils.Color):
            return list(value)
        if isinstance(value, mathutils.Euler):
            return list(value)
        if isinstance(value, mathutils.Matrix):
            return [list(row) for row in value]
        try:
            return float(value)
        except Exception:
            return str(value)


    def blender_list_materials():
        """List all Blender materials ending with _MAT"""
        return [m for m in bpy.data.materials if m.name.endswith("_MAT")]
    

    def blender_get_assigned_meshes(mat):
        """Return all meshes that use this material."""
        meshes = []
        for obj in bpy.data.objects:
            if obj.type == 'MESH':
                for slot in obj.material_slots:
                    if slot.material and slot.material.name == mat.name:
                        meshes.append(obj.name)
                        break
        return sorted(list(dict.fromkeys(meshes)))


    def blender_collect_graph(mat):
        """Collect node data and links from material.node_tree"""
        if not mat.use_nodes or not mat.node_tree:
            props = {"use_nodes": False}
            for p in ("diffuse_color", "specular_intensity", "metallic", "roughness"):
                if hasattr(mat, p):
                    try:
                        props[p] = safe_to_python(getattr(mat, p))
                    except Exception:
                        pass
            return {"nodes": {}, "links": [], "material_props": props}

        tree = mat.node_tree
        nodes = {}
        links = []

        for node in tree.nodes:
            ndata = {
                "type": node.bl_idname,
                "name": node.name,
                "label": getattr(node, "label", ""),
                "location": safe_to_python(node.location[:]),
                "width": getattr(node, "width", None),
                "height": getattr(node, "height", None)
            }

            # Special node handling
            if node.bl_idname == "ShaderNodeTexImage":
                img = getattr(node, "image", None)
                if img:
                    ndata["image"] = getattr(img, "filepath", None)
            elif node.bl_idname == "ShaderNodeGroup":
                ndata["group_name"] = node.node_tree.name
            elif node.bl_idname == "ShaderNodeScript":
                ndata["script_path"] = getattr(node, "filepath", "")

            # Collect inputs (default values of unlinked sockets)
            inputs = {}
            for inp in node.inputs:
                if not inp.is_linked:
                    try:
                        val = getattr(inp, "default_value", None)
                        if val is not None:
                            inputs[inp.name] = safe_to_python(val)
                    except Exception as e:
                        inputs[inp.name] = f"<unreadable: {type(e).__name__}>"
            ndata["inputs"] = inputs

            nodes[node.name] = ndata

        # Collect links
        for link in tree.links:
            try:
                links.append({
                    "src_node": link.from_node.name,
                    "src_socket": link.from_socket.name,
                    "dst_node": link.to_node.name,
                    "dst_socket": link.to_socket.name
                })
            except Exception:
                pass  # ignore broken links

        return {"nodes": nodes, "links": links}


    def blender_build_shader_entry(mat):
        """Build the full shader entry for export."""
        return {
            "name": mat.name,
            "engine": "blender",
            "graph": blender_collect_graph(mat),
            "assignments": blender_get_assigned_meshes(mat)
        }


# ----------------------------
# Main exporter
# ----------------------------
def export_surfacing(out_json_path):
    result = {"shaders": {}, "meta": {"dcc": "unknown"}}

    if IN_MAYA:
        result["meta"]["dcc"] = "maya"
        mats = maya_list_materials()
        print(f"[export_surfacing] Found {len(mats)} materials in Maya matching *_MAT")
        for m in mats:
            try:
                entry = maya_build_shader_entry(m)
                result["shaders"][m] = entry
            except Exception as e:
                print(f"[export_surfacing] Error processing material {m}: {e}")

    elif IN_BLENDER:
        result["meta"]["dcc"] = "blender"
        mats = blender_list_materials()
        print(f"[export_surfacing] Found {len(mats)} materials in Blender matching *_MAT")
        for m in mats:
            try:
                entry = blender_build_shader_entry(m)
                result["shaders"][m.name] = entry
            except Exception as e:
                print(f"[export_surfacing] Error processing material {m.name}: {e}")

    else:
        raise RuntimeError("Neither Maya nor Blender detected in this Python environment.")

    write_json(result, out_json_path)
    return result

# export_surfacing("A:\ICHIGO\surfacing_export.json")