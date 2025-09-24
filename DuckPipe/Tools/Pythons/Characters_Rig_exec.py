"""
Exec pour BLENDER et MAYA
"""

import os
import sys


DEPT_SUFFIX = "_rig"
REFNODS = ["{node_dlv_path}/{node_name}_body.fbx",
           "{node_dlv_path}/{node_name}_cfx_prez.fbx",
           "{node_dlv_path}/{node_name}_cfx.fbx",
           "{node_dlv_path}/{node_name}_model_helpers.fbx",
           "{node_dlv_path}/{node_name}_groom.fbx"
           ]

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
        MayaProcs.reset_scene(f"{PROD_PATH}/Assets/Templates/Characters_Rig_template.ma")
                                
    elif IN_BLENDER:
        pass
        # des procs dans blender
    

def execute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> execute")

    if IN_MAYA:
        # créer le group REF si nécessaire
        ref_grp_name = "__REF__"
        if not cmds.objExists(ref_grp_name):
            ref_grp_name = cmds.group(n=ref_grp_name, empty=True)

        # importer ou référencer les FBX
        for node_template in REFNODS:
            fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
            MayaProcs.reference_fbx(fbx_path, ref_grp_name)
            

    elif IN_BLENDER:
        pass
        # des procs dans blender


def postexecute():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-execute")

    if IN_MAYA:
        cmds.file(rename=EXECUTED_FILE)
        cmds.file(save=True, type="mayaAscii", force=True)

    elif IN_BLENDER:
        bpy.ops.wm.save_mainfile()
    

# --- MAIN---
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

def main():        
    preexecute()
    execute()
    postexecute()

if __name__ == "__main__":
    main()
