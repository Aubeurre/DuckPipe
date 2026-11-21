"""
Exec pour MAYA
"""

import os
import sys

# ------------------------------------------------------
# Ajout du répertoire courant au path
# ------------------------------------------------------
if "__file__" not in globals():
    __file__ = sys.argv[1]

current_dir = os.path.dirname(__file__)
if current_dir not in sys.path:
    sys.path.append(current_dir)

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------
DEPT_SUFFIX = "_cfxshot"
TEMPLATE_FILE = "Shots_CfxShot_template"

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
# Detection environnement FORCE MAYA
# ------------------------------------------------------

import maya.cmds as cmds
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs
from Sub_Procs import Assembly_import
from Sub_Procs import repathAllMayaSceneInEnvVar

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
# Chemins et variables dérivées
# ------------------------------------------------------
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)
local_dlv_path = dlv_path.replace("\\", "/").replace(PROD_PATH, LOCAL_PATH)
seq_root_path = os.path.dirname(os.path.dirname(os.path.dirname(dlv_path)))
seq_dlv_path = os.path.join(seq_root_path, 'dlv')
template_path = os.path.join(os.path.dirname(os.path.dirname(asset_root_path)), "Template")
studio_template_path = template_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

print("--------------------------------")
debug_vars = {
    "EXECUTED_FILE": EXECUTED_FILE,
    "SCRIPT_FILE": SCRIPT_FILE,
    "PROD_PATH": PROD_PATH,
    "LOCAL_PATH": LOCAL_PATH,
    "file_root": file_root,
    "asset_path": asset_path,
    "asset_root_path": asset_root_path,
    "dlv_path": dlv_path,
    "asset_name": asset_name,
    "studio_dlv_path": studio_dlv_path,
    "seq_root_path": seq_root_path,
    "seq_dlv_path": seq_dlv_path,
}
print("\n========== DEBUG VARS ==========")
for k, v in debug_vars.items():
    print(f"{k:<20} : {v}")
print("=================================\n")

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

    MayaProcs.cleanReferencesBeforeSave()
    cmds.file(rename=EXECUTED_FILE)
    cmds.file(save=True, type="mayaAscii", force=True)


def postexecute():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-execute")
        
    repathAllMayaSceneInEnvVar.repath_ma_file(EXECUTED_FILE, PROD_PATH.split('/')[-1])

# ------------------------------------------------------
# CUSTOM
# ------------------------------------------------------


# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():
    print('ici')
    preexecute()
    execute()
    postexecute()
    print('DONE SAVED WITH SUCCESS')


if __name__ == "__main__":
    main()

