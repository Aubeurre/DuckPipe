# -*- coding: utf-8 -*-
"""
Importeur surfacing JSON -> reconstruit shaders et assignations
Supporte : Maya (maya.cmds) et Blender (bpy).

Usage:
    # Maya (batch or UI)
   import Surfacing_import
    Surfacing_import.import_surfacing("C:/path/surfacing.json")

    # Blender (with bpy available)
   import Surfacing_import
    Surfacing_import.import_surfacing("C:/path/surfacing.json")

Notes:
- Le JSON attendu est celui produit par export_surfacing.py (structure: { "shaders": { name: { "engine":..., "graph":..., "assignments":[...] } } })
- Reconstruction approximative : certains attributs/arrays/plug types complexes sont ignorés ou loggués.
"""

import os
import sys
import maya.cmds as cmds
from pprint import pformat
from Soft_Procs import GlobalProcs

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



def safe_name(name):
    # simple sanitation
    return str(name).replace(" ", "_")


# -------------------------
# Maya importer
# -------------------------
if IN_MAYA:

    def create_lambert(name):
        shader = cmds.shadingNode("lambert", asShader=True, name=f"{name}_LAMBERT")
        sg = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name=f"{name}_LAMBERT_SG")
        cmds.connectAttr(shader + ".outColor", sg + ".surfaceShader", force=True)
        return shader, sg


    def assign_to_meshes(sg, meshes):
        for m in meshes:
            if not cmds.objExists(m):
                print(f"[WARN] Mesh not found: {m}")
                continue
            shapes = cmds.listRelatives(m, shapes=True, fullPath=True) or [m]
            for s in shapes:
                try:
                    cmds.sets(s, e=True, forceElement=sg)
                except Exception as e:
                    print(f"[ERR] Cannot assign {sg} to {s}: {e}")


    def maya_connect_texture(lambert, image_path, is_udim=False):
        file_node = cmds.shadingNode("file", asTexture=True, name=lambert + "_TEX")
        place = cmds.shadingNode("place2dTexture", asUtility=True, name=lambert + "_PLACE2D")

        # Connexions place2dTexture file
        connections = [
            ("coverage", "coverage"), ("translateFrame", "translateFrame"), ("rotateFrame", "rotateFrame"),
            ("mirrorU", "mirrorU"), ("mirrorV", "mirrorV"), ("stagger", "stagger"), ("wrapU", "wrapU"),
            ("wrapV", "wrapV"), ("repeatUV", "repeatUV"), ("offset", "offset"), ("rotateUV", "rotateUV"),
            ("noiseUV", "noiseUV"), ("vertexUvOne", "vertexUvOne"), ("vertexUvTwo", "vertexUvTwo"),
            ("vertexUvThree", "vertexUvThree"), ("vertexCameraOne", "vertexCameraOne"),
            ("outUV", "uv"), ("outUvFilterSize", "uvFilterSize")
        ]
        for src, dst in connections:
            try:
                cmds.connectAttr(f"{place}.{src}", f"{file_node}.{dst}", force=True)
            except:
                pass

        # Remplacer <UDIM> par #### si necessaire
        if is_udim:
            cmds.setAttr(file_node + ".uvTilingMode", 3)  # 3 = UDIM
            cmds.setAttr(file_node + ".uvTileProxyQuality", 1)
        else:
            if not os.path.exists(image_path):
                print(f"[WARN] Texture not found on disk: {image_path}")
                return None

        cmds.setAttr(file_node + ".fileTextureName", image_path, type="string")
        cmds.connectAttr(file_node + ".outColor", lambert + ".color", force=True)
        print(f"[IMPORT] Connected texture {image_path}")
        return file_node


    def make_lambert_transparent(lambert, amount=1.0):
        cmds.setAttr(lambert + ".transparency", amount, amount, amount, type="double3")
        print(f"[IMPORT] {lambert} set transparent={amount}")


    def shader_is_transparent(entry):
        graph = entry.get("graph", {})
        nodes = graph.get("nodes", {})
        links = graph.get("links", [])

        incoming = {}
        for l in links:
            dst = l.get("dst_node")
            src = l.get("src_node")
            if dst and src:
                incoming.setdefault(dst, []).append(src)

        # BFS depuis Material Output
        start = "Material Output"
        if start not in nodes:
            return False

        to_visit = incoming.get(start, [])
        visited = set()

        while to_visit:
            node = to_visit.pop(0)
            if node in visited:
                continue
            visited.add(node)

            info = nodes.get(node, {})
            ntype = info.get("type", "").lower()
            nname = node.lower()

            # Transparent BSDF
            if "transparent" in ntype or "transparent" in nname:
                return True

            # Mix Shader
            if "mix" in ntype:
                for s in incoming.get(node, []):
                    if s not in visited:
                        to_visit.append(s)
                continue

            # Autre
            for s in incoming.get(node, []):
                if s not in visited:
                    to_visit.append(s)

        return False


    def find_first_texture(entry):
        """Retourne le premier chemin de texture trouve dans le graph et si c'est un UDIM."""
        graph = entry.get("graph", {})
        nodes = graph.get("nodes", {})

        for nname, ninfo in nodes.items():
            # Pour Maya: fileTextureName dans attributes
            img = ninfo.get("attributes", {}).get("fileTextureName")
            # Pour Blender: image directement dans le node
            if not img:
                img = ninfo.get("image")

            if img:
                # Detection UDIM directement sur le path
                is_udim = "<UDIM>" in img
                return img, is_udim

        return None, False


    def import_light_shaders(json_path, texture_root=None):
        data = GlobalProcs.read_json(json_path)
        shaders = data.get("shaders", {})
        print(f"[IMPORT] Found {len(shaders)} shaders.")

        for mat_name, entry in shaders.items():
            print(f"\n[IMPORT] Processing {mat_name}")
            lambert, sg = create_lambert(mat_name)

            # -----------------------------
            # 1. DETECT TEXTURE
            # -----------------------------        
            tex_path, is_udim = find_first_texture(entry)
            if tex_path:
                if is_udim:
                    print(f"[IMPORT] UDIM texture detected: {tex_path}")
                else:
                    print(f"[IMPORT] Texture found: {tex_path}")
            
                # Remplacer <UDIM> par #### pour Maya si necessaire
                if is_udim:
                    maya_connect_texture(lambert, tex_path, is_udim=True)
                else:
                    maya_connect_texture(lambert, tex_path)
                    
                # --- FORCER LE VIEWPORT A AFFICHER LA TEXTURE ---
                
            else:
                print("[IMPORT] No texture in this material")

            # DETECT TRANSPARENCY
            if shader_is_transparent(entry):
                print("[IMPORT] Shader is transparent - Adding transparency to lambert")
                make_lambert_transparent(lambert, amount=1.0)

            # -----------------------------
            # 2. ASSIGN TO MESHES
            # -----------------------------
            meshes = entry.get("assignments", [])
            assign_to_meshes(sg, meshes)

        print("\n[IMPORT] Light surfacing import complete!")


    def maya_create_node(ntype, name=None):
        name = name or cmds.createNode(ntype)
        try:
            node = cmds.createNode(ntype, name=name)
        except Exception:
            # maybe node exists or type not creatable that way: try createNode without name
            try:
                node = cmds.createNode(ntype)
            except Exception as e:
                print(f"[MAYA] Failed to create node type '{ntype}': {e}")
                return None
        return node


    def maya_set_attribute(node, attr, val):
        plug = f"{node}.{attr}"
        try:
            # string
            if isinstance(val, str):
                cmds.setAttr(plug, val, type="string")
            # tuple/list of size 3 or 4 -> set as vector/color
            elif isinstance(val, (list, tuple)):
                if len(val) == 3:
                    cmds.setAttr(plug, *val, type="double3")
                elif len(val) == 4:
                    cmds.setAttr(plug, *val, type="double4")
                else:
                    # try generic setAttr by index if attribute exists as array? skip for now
                    try:
                        cmds.setAttr(plug, val)
                    except Exception:
                        pass
            else:
                # numeric (int/float/bool)
                cmds.setAttr(plug, val)
            return True
        except Exception as e:
            print(f"[MAYA] Could not set attr {plug} = {val} ({type(val)}): {e}")
            return False


    def maya_rebuild_graph(graph):
        """
        graph = {"nodes": {nodeName: {type, attributes...}}, "connections":[{src_node, src_attr, dst_node, dst_attr}]}
        Returns mapping oldNodeName -> createdNodeName
        """
        nodes = graph.get("nodes", {})
        conns = graph.get("connections", [])

        created = {}  # original name -> new node name

        # create nodes
        for orig_name, info in nodes.items():
            ntype = info.get("type", None)
            # sanitize name for creation
            base = safe_name(orig_name)
            # Try to create node of given type with original-ish name
            created_name = base
            # if node type is 'unknown' we fallback to transform empty group or shading node?
            try:
                new = maya_create_node(ntype, name=created_name)
            except Exception:
                new = None

            if new is None:
                # fallback: create a transform to hold attrs if needed
                try:
                    new = cmds.createNode("transform", name=created_name + "_TR")
                except Exception:
                    new = created_name  # last resort
            created[orig_name] = new

            # set attributes
            attrs = info.get("attributes", {})
            for a_name, a_val in attrs.items():
                # skip typical read-only or connection-only attrs if they fail
                maya_set_attribute(new, a_name, a_val)

        # create connections
        for c in conns:
            try:
                src = created.get(c["src_node"], c["src_node"])
                dst = created.get(c["dst_node"], c["dst_node"])
                src_attr = c["src_attr"]
                dst_attr = c["dst_attr"]
                src_plug = f"{src}.{src_attr}"
                dst_plug = f"{dst}.{dst_attr}"
                # ensure plugs exist? try to connect
                cmds.connectAttr(src_plug, dst_plug, force=True)
            except Exception as e:
                print(f"[MAYA] Warning cannot connect {c}: {e}")

        return created


    def maya_assign_material_to_mesh(mat_node_name, mesh_names):
        """
        mat_node_name: name of shader node or material name
        mesh_names: list of full-path transforms or shapes
        """
        # make/get shadingEngine
        sg_name = f"{mat_node_name}_SG"
        if not cmds.objExists(sg_name):
            try:
                sg = cmds.sets(name=sg_name, renderable=True, noSurfaceShader=True, empty=True)
            except Exception:
                try:
                    sg = cmds.createNode("shadingEngine", name=sg_name)
                except Exception as e:
                    print(f"[MAYA] Cannot create shadingEngine {sg_name}: {e}")
                    sg = None
        else:
            sg = sg_name

        # connect shader to SG: try common plug names
        # find shader out attribute candidates
        out_attrs = ["outColor", "out", "outColor", "surfaceShader", "outSurface"]
        connected = False
        for out_attr in out_attrs:
            try:
                if cmds.objExists(f"{mat_node_name}.{out_attr}"):
                    # connect to SG
                    try:
                        cmds.connectAttr(f"{mat_node_name}.{out_attr}", f"{sg}.surfaceShader", force=True)
                        connected = True
                        break
                    except Exception:
                        # try connect to first available
                        pass
            except Exception:
                continue

        # if not connected, attempt generic connection to surfaceShader if shader has outColor
        if not connected:
            try:
                if cmds.objExists(f"{mat_node_name}.outColor"):
                    cmds.connectAttr(f"{mat_node_name}.outColor", f"{sg}.surfaceShader", force=True)
                    connected = True
            except Exception:
                pass

        # assign meshes (ensure shapes)
        for m in mesh_names:
            try:
                # If m is a transform, get shapes
                shapes = []
                if cmds.objExists(m):
                    if cmds.nodeType(m).endswith("Shape"):
                        shapes = [m]
                    else:
                        shapes = cmds.listRelatives(m, shapes=True, fullPath=True) or []
                # if shapes empty, try to add transform itself
                if not shapes:
                    shapes = [m]

                for s in shapes:
                    try:
                        cmds.sets(s, e=True, forceElement=sg)
                    except Exception as e:
                        print(f"[MAYA] Cannot assign {sg} to {s}: {e}")
            except Exception as e:
                print(f"[MAYA] assign error for {m}: {e}")


    def import_surfacing_maya(json_path):
        data = GlobalProcs.read_json(json_path)
        shaders = data.get("shaders", {})
        created_map = {}
        for mat_name, entry in shaders.items():
            print(f"[MAYA] Reconstructing shader '{mat_name}'")
            graph = entry.get("graph") or {}
            # graph contains nodes and connections; we will recreate upstream nodes and finally connect the node that corresponds to mat_name
            created = maya_rebuild_graph(graph)
            created_map[mat_name] = created
            # if the material name is itself a node in graph, get created node
            main_node = created.get(mat_name, mat_name)
            # assign to meshes
            assignments = entry.get("assignments", [])
            if assignments:
                maya_assign_material_to_mesh(main_node, assignments)
        print("[MAYA] Import complete.")


# -------------------------
# Blender importer
# -------------------------
if IN_BLENDER:
    def blender_create_node(tree, bl_idname, node_name=None):
        try:
            node = tree.nodes.new(bl_idname)
        except Exception as e:
            print(f"[BLENDER] Cannot create node {bl_idname}: {e}")
            return None
        if node_name:
            node.name = node_name
        return node


    def blender_set_input_default(node, input_name, val):
        # find input socket by name (or by index fallback)
        try:
            sock = node.inputs.get(input_name)
            if sock is None:
                # try by index or nearest
                for s in node.inputs:
                    if s.name == input_name:
                        sock = s
                        break
            if sock is None:
                return False
            # assign value
            if isinstance(val, (list, tuple)):
                try:
                    sock.default_value = tuple(val)
                except Exception:
                    try:
                        sock.default_value = list(val)
                    except Exception:
                        sock.default_value = val
            else:
                sock.default_value = val
            return True
        except Exception as e:
            print(f"[BLENDER] Cannot set input {node.name}.{input_name} = {val}: {e}")
            return False


    def blender_build_graph(mat_entry):
        """
        mat_entry['graph'] expected to be from export: { nodes: {name: {type,label,location,inputs}}, links: [...]}
        Recreate nodes, set default values, then recreate links.
        """
        graph = mat_entry.get("graph", {})
        nodes_data = graph.get("nodes", {})
        links = graph.get("links", [])

        # ensure material uses nodes
        mat_name = mat_entry.get("name")
        if isinstance(mat_name, str):
            mat = bpy.data.materials.get(mat_name) or bpy.data.materials.new(mat_name)
        else:
            # if exporter used material object, fallback
            mat = bpy.data.materials.new("MAT_from_json")

        mat.use_nodes = True
        tree = mat.node_tree
        # clear existing nodes
        for n in tree.nodes:
            tree.nodes.remove(n)

        created = {}
        # create nodes
        for orig_name, info in nodes_data.items():
            btype = info.get("type", None)  # e.g. ShaderNodeTexImage
            node = blender_create_node(tree, btype, node_name=orig_name)
            if node is None:
                continue
            # set inputs defaults
            inputs = info.get("inputs", {})
            for inp_name, inp_val in inputs.items():
                blender_set_input_default(node, inp_name, inp_val)
            # special handling for image nodes
            if info.get("image"):
                try:
                    img_path = info.get("image")
                    if img_path and os.path.exists(img_path):
                        img = bpy.data.images.load(img_path)
                        if hasattr(node, "image"):
                            node.image = img
                except Exception as e:
                    print(f"[BLENDER] Could not load image {info.get('image')}: {e}")
            created[orig_name] = node

        # create links
        for l in links:
            try:
                src_node = created.get(l["src_node"])
                dst_node = created.get(l["dst_node"])
                if not src_node or not dst_node:
                    continue
                # find sockets
                src_sock = None
                dst_sock = None
                # try by name
                src_sock = src_node.outputs.get(l.get("src_socket"))
                dst_sock = dst_node.inputs.get(l.get("dst_socket"))
                if not src_sock or not dst_sock:
                    # fallback to first sockets
                    src_sock = next(iter(src_node.outputs), None)
                    dst_sock = next(iter(dst_node.inputs), None)
                if src_sock and dst_sock:
                    tree.links.new(src_sock, dst_sock)
            except Exception as e:
                print(f"[BLENDER] Cannot create link {l}: {e}")

        return mat


    def blender_assign_material_to_mesh(mat, mesh_names):
        for obj_name in mesh_names:
            obj = bpy.data.objects.get(obj_name)
            if not obj:
                print(f"[BLENDER] Object not found for assignment: {obj_name}")
                continue
            if obj.type != 'MESH':
                print(f"[BLENDER] Not a mesh, skipping: {obj_name}")
                continue
            # ensure material slot
            if mat.name not in [m.name for m in obj.data.materials if m]:
                # append new material slot
                obj.data.materials.append(mat)
            else:
                # already assigned; ensure first slot is the material
                pass


    def import_surfacing_blender(json_path):
        data = GlobalProcs.read_json(json_path)
        shaders = data.get("shaders", {})
        for mat_name, entry in shaders.items():
            print(f"[BLENDER] Reconstructing material '{mat_name}'")
            mat = blender_build_graph(entry)
            assignments = entry.get("assignments", [])
            blender_assign_material_to_mesh(mat, assignments)
        print("[BLENDER] Import complete.")


# -------------------------
# Public 
# -------------------------
def import_surfacing(json_path):
    if not os.path.exists(json_path):
        raise FileNotFoundError(json_path)
    if IN_MAYA:
        import_surfacing_maya(json_path)
    elif IN_BLENDER:
        import_surfacing_blender(json_path)


def import_light_surfacing(json_path):
    if not os.path.exists(json_path):
        raise FileNotFoundError(json_path)
    if IN_MAYA:
        import_light_shaders(json_path)
        
#import_surfacing("A:\ICHIGO\surfacing_export.json")
#import_light_surfacing("A:\ICHIGO\surfacing_export.json")