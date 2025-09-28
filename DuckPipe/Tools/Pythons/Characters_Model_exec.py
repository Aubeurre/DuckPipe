"""
Exec pour BLENDER et MAYA
"""

import os
import sys

current_dir = os.path.dirname(__file__)
if current_dir not in sys.path:
    sys.path.append(current_dir)

REFNODS = ["{node_dlv_path}{node_name}"] # y en a pas pour le modeling
DEPT_SUFFIX = "_model"
TEMPLATE_FILE = "Characters_Model_template"

if "--" in sys.argv:
    idx = sys.argv.index("--")
    extra_args = sys.argv[idx+1:]
    if extra_args:
        server_file_path = extra_args[0]
        print("Fichier reçu :", server_file_path)

# ------------------------------------------------------
# Detection environnement
# ------------------------------------------------------
IN_BLENDER = False
IN_MAYA = False

try:
    import bpy

    current_dir = os.path.dirname(__file__)
    if current_dir not in sys.path:
        sys.path.append(current_dir)

    import BlenderProcs
    import GlobalProcs
    
    IN_BLENDER = True
    EXECUTED_FILE = bpy.data.filepath
    SCRIPT_FILE = os.path.abspath(__file__)
    PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
    LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)
    
    print("_____________________")
    print("EXECUTED_FILE -> ", EXECUTED_FILE)
    print("SCRIPT_FILE -> ", SCRIPT_FILE)
    print("PROD_PATH -> ", PROD_PATH)
    print("LOCAL_PATH -> ", LOCAL_PATH)
    print("_____________________")

except ImportError:
    pass

try:
    import maya.cmds as cmds

    python_file = sys.argv[1]
    current_dir = os.path.dirname(python_file)
    if current_dir not in sys.path:
        sys.path.append(current_dir)
        
    import MayaProcs
    import GlobalProcs

    IN_MAYA = True
    EXECUTED_FILE = cmds.file(q=True, sn=True)
    SCRIPT_FILE = python_file
    PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
    LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)

    print("_____________________")
    print("EXECUTED_FILE -> ", EXECUTED_FILE)
    print("SCRIPT_FILE -> ", SCRIPT_FILE)
    print("PROD_PATH -> ", PROD_PATH)
    print("LOCAL_PATH -> ", LOCAL_PATH)
    print("_____________________")

except ImportError:
    pass
    

# ------------------------------------------------------
# Fonction commune
# ------------------------------------------------------
def preexecute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> Pre-execute")

    if IN_MAYA:
        # nouvelle scene
        MayaProcs.reset_scene(f"{template_path}/{TEMPLATE_FILE}.ma")
            
    elif IN_BLENDER:        
        # nouvelle scene
        BlenderProcs.reset_scene(f"{template_path}/{TEMPLATE_FILE}.blend")
    

def execute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> execute")

    if IN_MAYA:
        # creer le group REF si necessaire
        ref_grp_name = "REF"
        if not cmds.objExists(ref_grp_name):
            ref_grp_name = cmds.group(n=ref_grp_name, empty=True)

        # importer ou referencer les FBX
        for node_template in REFNODS:
            fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
            MayaProcs.reference_fbx(fbx_path, ref_grp_name)
            

    elif IN_BLENDER:
        # créer la collection REF si necessaire
        ref_col_name = "REF"
        if ref_col_name not in bpy.data.collections:
            ref_collection = bpy.data.collections.new(ref_col_name)
            bpy.context.scene.collection.children.link(ref_collection)
        else:
            ref_collection = bpy.data.collections[ref_col_name]

        # importer les FBX
        for node_template in REFNODS:
            fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
            # BlenderProcs.reference_fbx(ref_collection, ref_col_name) y en a pas au model


def postexecute():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-execute")

    if IN_MAYA:
        cmds.file(rename=EXECUTED_FILE)
        cmds.file(save=True, type="mayaAscii", force=True)

    elif IN_BLENDER:
        bpy.ops.wm.save_as_mainfile(filepath=EXECUTED_FILE)
    

# --- MAIN---
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

# server
if server_file_path:
    all_asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(server_file_path)))))
    template_path = os.path.join(all_asset_root_path, "Template")

def main():      
    preexecute()
    execute()
    postexecute()

if __name__ == "__main__":
    main()
