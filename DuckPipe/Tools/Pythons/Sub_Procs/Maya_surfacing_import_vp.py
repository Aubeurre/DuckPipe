import json
import maya.cmds as cmds
import os

def load_json(path):
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)

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
    data = load_json(json_path)
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
    
def import_surf(path):
    import_light_shaders(path)
    # import maya.mel as mel
    # mel.eval("generateAllUvTilePreviews();") Ne batch pas
