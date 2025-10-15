"""
Exec pour BLENDER et MAYA
"""

import os
import sys

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------
REFNODS = ["{node_dlv_path}{node_name}"]
DEPT_SUFFIX = "_dept"
TEMPLATE_FILE = "{Type}_{Dept}_template"

# ------------------------------------------------------
# Gestion des arguments
# ------------------------------------------------------
server_file_path = None
if "--" in sys.argv:
    idx = sys.argv.index("--")
    extra_args = sys.argv[idx + 1:]
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

except ImportError:
    pass    

# ------------------------------------------------------
# Chemins et variables dérivées
# ------------------------------------------------------
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(EXECUTED_FILE))
asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)
template_path = os.path.join(asset_root_path, "Template")

print("--------------------------------")
for item in [
EXECUTED_FILE, SCRIPT_FILE, PROD_PATH, LOCAL_PATH,
asset_path, asset_root_path, dlv_path, asset_name,
studio_dlv_path, template_path
]:
    print(item)

# ------------------------------------------------------
# Fonction commune
# ------------------------------------------------------
def preexecute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> Pre-execute")

    if IN_MAYA:
        MayaProcs.reset_scene(f"{template_path}/{TEMPLATE_FILE}.ma")
    elif IN_BLENDER:
        BlenderProcs.reset_scene(f"{template_path}/{TEMPLATE_FILE}.blend")
    

def execute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> execute")

    if IN_MAYA:
        # importer ou referencer les FBX
        for node_template in REFNODS:
            fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
            MayaProcs.reference_fbx(fbx_path, "REF")
    elif IN_BLENDER:
        # importer les FBX
        for node_template in REFNODS:
            fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
            BlenderProcs.reference_fbx(fbx_path, "REF")


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
    

# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():
    preexecute()
    execute()
    postexecute()


if __name__ == "__main__":
    main()
