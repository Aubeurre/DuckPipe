"""
Publish pour BLENDER et MAYA
"""

import os
import sys
import shutil

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------
TRASHLIST = ['TRASH']
DEPT_SUFFIX = "_model_OK"

# ------------------------------------------------------
# Gestion des arguments
# ------------------------------------------------------
server_file_path = None
if "--" in sys.argv:
    idx = sys.argv.index("--")
    extra_args = sys.argv[idx + 1:]
    if extra_args:
        server_file_path = extra_args[0]
        print("Fichier recu :", server_file_path)

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

    from Soft_Procs import BlenderProcs
    from Soft_Procs import GlobalProcs

    IN_BLENDER = True
    EXECUTED_FILE = bpy.data.filepath
    SCRIPT_FILE = os.path.abspath(__file__)
    PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
    LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)

except ImportError:
    pass

try:
    import maya.cmds as cmds

    python_file = sys.argv[1]

    current_dir = os.path.dirname(python_file)
    if current_dir not in sys.path:
        sys.path.append(current_dir)

    from Soft_Procs import MayaProcs
    from Soft_Procs import GlobalProcs
    
    IN_MAYA = True
    EXECUTED_FILE = cmds.file(q=True, sn=True)
    SCRIPT_FILE = python_file
    PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
    LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)

except ImportError:
    pass    

# ------------------------------------------------------
# Chemins et variables derivees
# ------------------------------------------------------
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(EXECUTED_FILE))
asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

print("--------------------------------")
debug_vars = {
    "EXECUTED_FILE": EXECUTED_FILE,
    "SCRIPT_FILE": SCRIPT_FILE,
    "PROD_PATH": PROD_PATH,
    "LOCAL_PATH": LOCAL_PATH,
    "asset_path": asset_path,
    "asset_root_path": asset_root_path,
    "dlv_path": dlv_path,
    "asset_name": asset_name,
    "studio_dlv_path": studio_dlv_path,
}

print("\n----- DEBUG -----")
for name, value in debug_vars.items():
    print(f"{name:<18} = {value}")
print("-----------------\n")

    
# ------------------------------------------------------
# Fonction commune
# ------------------------------------------------------
def prepublish():
    """
    Tout ce qui se passe ici se fait dans la scene de OK
    """
    print("Pre-publish")

    if IN_MAYA:
        # des procs dans maya
        pass
    elif IN_BLENDER:
        # des procs dans blender
        pass


def publish():
    """
    Tout ce qui se passe ici se fait dans la scene de OK
    """
    print("publish")
    
    # on a juste besoin de sortir les fbx et compagniem on ne va pas gerer de cene OK
    export_list = [
        [['BODY_GRP'], f'{dlv_path}/{asset_name}_body.fbx'],
        [['CFX_GRP'], f'{dlv_path}/{asset_name}_cfx.fbx'],
        [['HELPERS_GRP'], f'{dlv_path}/{asset_name}_model_helpers.fbx'],
        [['BODY_GRP','CFX_GRP'], f'{dlv_path}/{asset_name}_surf.fbx'],
    ]

    if IN_MAYA:
        for grp, path in export_list:
            MayaProcs.export_hierarchy_by_name(grp, path)
        # MayaProcs.clean_publish(TRASHLIST)
        # cmds.file(save=True, type="mayaAscii")
    elif IN_BLENDER:
        BlenderProcs.confo_from_blender()
        for grp, path in export_list:
            BlenderProcs.export_hierarchy_by_name(grp, path)
        # bpy.ops.wm.save_mainfile()


def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste, scene fermee
    """
    print("Post-publish")
            
    export_list = [
        f'{dlv_path}/{asset_name}_body.fbx',
        f'{dlv_path}/{asset_name}_cfx.fbx',
        f'{dlv_path}/{asset_name}_model_helpers.fbx',
        f'{dlv_path}/{asset_name}_surf.fbx',
    ]

    for file_path in export_list:
        if os.path.exists(file_path):
            dest_path = os.path.join(studio_dlv_path, os.path.basename(file_path)).replace("\\", "/")
            shutil.copy2(file_path, dest_path)
            print(f"[postpublish] Copied {file_path} -> {dest_path}")
    
        
# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():        
    prepublish()
    publish()
    postpublish()

if __name__ == "__main__":
    main()
