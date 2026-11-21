import maya.cmds as cmds
import os
import re

ENV_VAR = "DUCKPIPE_ROOT"

# --------------------------------------------------
# UTILS
# --------------------------------------------------
def to_env_path(path, prodname):
    if not path:
        return path

    path = path.replace("\\", "/")

    if f"${{{ENV_VAR}}}" in path:
        return path  # déjà bon

    idx = path.find(prodname)
    if idx == -1:
        return path  # pas concerné

    return f"${{{ENV_VAR}}}/" + path[idx:]


def safe_set_attr(node, attr, value):
    try:
        cmds.setAttr(f"{node}.{attr}", value, type="string")
        return True
    except:
        return False


# --------------------------------------------------
# REFERENCES
# --------------------------------------------------
def repath_references(prodname):
    refs = cmds.file(q=True, r=True) or []

    for ref_path in refs:
        new_path = to_env_path(ref_path, prodname)

        if new_path == ref_path:
            continue

        try:
            ref_node = cmds.referenceQuery(ref_path, referenceNode=True)
            cmds.file(new_path, loadReference=ref_node)
            print(f"[REF] {ref_path} -> {new_path}")
        except Exception as e:
            print(f"[REF ERROR] {ref_path} : {e}")


# --------------------------------------------------
# FILE NODES (textures)
# --------------------------------------------------
def repath_file_nodes(prodname):
    nodes = cmds.ls(type="file") or []

    for node in nodes:
        path = cmds.getAttr(f"{node}.fileTextureName")

        new_path = to_env_path(path, prodname)

        if new_path != path:
            if safe_set_attr(node, "fileTextureName", new_path):
                print(f"[FILE] {path} -> {new_path}")


# --------------------------------------------------
# ALEMBIC + GPU CACHE
# --------------------------------------------------
def repath_caches(prodname):
    nodes = cmds.ls(type=["AlembicNode", "gpuCache"]) or []

    for node in nodes:
        node_type = cmds.nodeType(node)

        attr = None
        if node_type == "AlembicNode":
            attr = "abc_File"
        elif node_type == "gpuCache":
            attr = "cacheFileName"

        if not attr:
            continue

        path = cmds.getAttr(f"{node}.{attr}")
        new_path = to_env_path(path, prodname)

        if new_path != path:
            if safe_set_attr(node, attr, new_path):
                print(f"[CACHE] {path} -> {new_path}")


# --------------------------------------------------
# IMAGE PLANE
# --------------------------------------------------
def repath_image_planes(prodname):
    nodes = cmds.ls(type="imagePlane") or []

    for node in nodes:
        path = cmds.getAttr(f"{node}.imageName")

        new_path = to_env_path(path, prodname)

        if new_path != path:
            if safe_set_attr(node, "imageName", new_path):
                print(f"[IMG] {path} -> {new_path}")


# --------------------------------------------------
# GENERIC SEARCH (fallback)
# --------------------------------------------------
def repath_generic(prodname):
    nodes = cmds.ls()

    for node in nodes:
        attrs = cmds.listAttr(node, usedAsFilename=True) or []

        for attr in attrs:
            try:
                path = cmds.getAttr(f"{node}.{attr}")
            except:
                continue

            if not isinstance(path, str):
                continue

            new_path = to_env_path(path, prodname)

            if new_path != path:
                if safe_set_attr(node, attr, new_path):
                    print(f"[GENERIC] {node}.{attr} -> {new_path}")


# --------------------------------------------------
# MAIN
# --------------------------------------------------
def repath_all(prodname):
    print("------ REPATH START ------")
    cmds.undoInfo(openChunk=True)
    repath_references(prodname)
    repath_file_nodes(prodname)
    repath_caches(prodname)
    repath_image_planes(prodname)
    repath_generic(prodname)
    cmds.undoInfo(closeChunk=True)

    print("------ REPATH DONE ------")

# --------------------------------------------------

def repath_ma_file(ma_path, prodname):
    if not os.path.exists(ma_path):
        print(f"[ERROR] File not found: {ma_path}")
        return

    print(f"[INFO] Processing: {ma_path}")

    new_lines = []
    changes = 0

    with open(ma_path, "r", encoding="utf-8") as f:
        for line in f:

            if prodname not in line:
                new_lines.append(line)
                continue

            parts = line.split('"')

            for i in range(1, len(parts), 2):
                path = parts[i].replace("\\", "/")

                if f"${{{ENV_VAR}}}" in path:
                    continue

                idx = path.find(prodname)
                if idx != -1:
                    new_path =f"${{{ENV_VAR}}}/{path[idx:]}"
                    parts[i] = new_path
                    changes += 1
                    print(f"[REPATH] {path} -> {new_path}")

            new_lines.append('"'.join(parts))

    with open(ma_path, "w", encoding="utf-8") as f:
        f.writelines(new_lines)

    print(f"[DONE] {changes} paths updated")