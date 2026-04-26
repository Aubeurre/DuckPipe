"""
Exec pour MAYA
"""

import os
import sys

# ------------------------------------------------------
# Ajout du repertoire courant au path
# ------------------------------------------------------
if "__file__" not in globals():
     __file__ = sys.argv[1]

current_dir = os.path.dirname(__file__)
if current_dir not in sys.path:
    sys.path.append(current_dir)

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------
REFNODS = ["{node_dlv_path}/{node_name}_model.fbx",
           "{node_dlv_path}/{node_name}_model_helpers.fbx"
           ]
DEPT_SUFFIX = "_rig"
TEMPLATE_FILE = "Props_Rig_template"

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

import maya.cmds as cmds
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs

IN_MAYA = True
python_file = sys.argv[1]
SCRIPT_FILE = python_file
current_dir = os.path.dirname(python_file)
if current_dir not in sys.path:
    sys.path.append(current_dir)

EXECUTED_FILE = cmds.file(q=True, sn=True)
PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)

# ------------------------------------------------------
# Chemins et variables derivees
# ------------------------------------------------------
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
root_asset_path = os.path.dirname(os.path.dirname(asset_root_path))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)
local_dlv_path = dlv_path.replace("\\", "/").replace(PROD_PATH, LOCAL_PATH)
local_template_path = os.path.join(asset_root_path, "Template")
template_path = os.path.join(root_asset_path, "Template").replace(LOCAL_PATH, PROD_PATH)

print("--------------------------------")
debug_vars = {
    "EXECUTED_FILE": EXECUTED_FILE,
    "SCRIPT_FILE": SCRIPT_FILE,
    "PROD_PATH": PROD_PATH,
    "LOCAL_PATH": LOCAL_PATH,
    "root_asset_path": root_asset_path,
    "asset_path": asset_path,
    "asset_root_path": asset_root_path,
    "dlv_path": dlv_path,
    "asset_name": asset_name,
    "studio_dlv_path": studio_dlv_path,
    "template_path": template_path,
}
    
# ------------------------------------------------------
# Fonction commune
# ------------------------------------------------------
def preexecute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> Pre-execute")

    MayaProcs.sanitize_ma(f"{template_path}/{TEMPLATE_FILE}.ma")
    MayaProcs.reset_scene(f"{template_path}/{TEMPLATE_FILE}.ma")
    

def execute():
    """
    Tout ce qui se passe ici se fait dans la scene de work
    """
    print(" -> execute")

    # importer ou referencer les FBX
    for node_template in REFNODS:
        fbx_path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
        MayaProcs.reference_fbx(fbx_path, "__REF__")

    MayaProcs.cleanReferencesBeforeSave()
    cmds.file(rename=EXECUTED_FILE)
    cmds.file(save=True, type="mayaAscii", force=True)


def postexecute():
    """
    Tout ce qui se passe ici se fait apres tout le reste, une fois la scene fermee
    """
    print(" -> Post-execute")
    reroot_fbx(EXECUTED_FILE)
    
# ------------------------------------------------------
# MAYA PROCS
# ------------------------------------------------------
def reroot_fbx(scene_path):

    print("REROOT:", scene_path)

    with open(scene_path, "r", encoding="utf-8") as f:
        print("lecture")
        lines = f.readlines()

    with open(scene_path, "w", encoding="utf-8") as f:
        print("ecriture")
        for line in lines:
            if "__dummy" in line:
                f.write(line.replace("__dummy", ""))
            else:
                f.write(line)
        f.flush()
        os.fsync(f.fileno())

    print("REROOT DONE.")

# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():
    preexecute()
    execute()
    postexecute()


if __name__ == "__main__":
    main()

