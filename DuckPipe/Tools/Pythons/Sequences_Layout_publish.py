"""
Publish pour MAYA
"""

import os
import sys

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------
TRASHLIST = ['TRASH']
DEPT_SUFFIX = '_layout_OK'

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

python_file = sys.argv[1]

current_dir = os.path.dirname(python_file)
if current_dir not in sys.path:
    sys.path.append(current_dir)

print("Maya detected")
from Soft_Procs import MayaProcs
print("MayaProcs imported")
from Soft_Procs import GlobalProcs
print("GlobalProcs imported")
from Sub_Procs import Layout_export
print("Layout_export imported")

IN_MAYA = True
EXECUTED_FILE = cmds.file(q=True, sn=True)
SCRIPT_FILE = python_file
PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)
LOCAL_PATH = GlobalProcs.get_local_path_from_filepath(EXECUTED_FILE, PROD_PATH)

# ------------------------------------------------------
# Chemins et variables derirees
# ------------------------------------------------------
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
asset_root_path = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(DEPT_SUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

print("--------------------------------")
for item in [
EXECUTED_FILE, SCRIPT_FILE, PROD_PATH, LOCAL_PATH,
asset_path, asset_root_path, dlv_path, asset_name,
studio_dlv_path
]:
    print(item)

    
# ------------------------------------------------------
# Fonction commune
# ------------------------------------------------------
def prepublish():
    """
    Tout ce qui se passe ici se fait dans la scene de OK
    """
    print("Pre-publish")


def publish():
    """
    Tout ce qui se passe ici se fait dans la scene de OK
    """
    print("publish")

    Layout_export.export(os.path.join(dlv_path, "layout.json"))
            
    full_scene_path = os.path.join(dlv_path, file_name).replace("\\", "/")
    cmds.file(rename=full_scene_path)
    cmds.file(save=True, type="mayaAscii", prompt=False)


def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print("Post-publish")

    MayaProcs.clean_publish(TRASHLIST)
    cmds.file(save=True, type="mayaAscii")
    
        
# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():        
    prepublish()
    publish()
    postpublish()

if __name__ == "__main__":
    main()
