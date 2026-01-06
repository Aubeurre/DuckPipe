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
REFNODS = ["{node_dlv_path}/{node_name}_cam.ma"
           ]
DEPT_SUFFIX = "_layout"
TEMPLATE_FILE = "Shots_Layout_template"

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
template_path = os.path.join(asset_root_path, "Template")
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
    # importer la cam
    for node_template in REFNODS:
        path = node_template.replace("{node_dlv_path}", studio_dlv_path).replace("{node_name}", asset_name)
        MayaProcs.reference_scene(path, "REF")

def postexecute():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-execute")
    
    # GESTION DES DEPENDANCES
    # on lis les dependences et on regarde si y a un Environment a importer
    print('GO ASSET DEPENDENCIES:')
    for item in get_asset_dependencies(asset_path):
        if item['type'] == 'Environments':
            asset_name = item['name']
            dlv_path = item['path']
            assembly_path = os.path.join(dlv_path, "assembly.json").replace("\\", "/")
            if os.path.exists(assembly_path):
                Assembly_import.import_assembly_for_anim(assembly_path, PROD_PATH)
                print(f"[postexecute] Imported assembly for {asset_name}")
            else:
                print(f"[postexecute] No assembly.json found for {asset_name} at {assembly_path}")
        # on rajoute aussi les rigs de persos et props
        if item['type'] in ['Characters', 'Props']:
            asset_name = item['name']
            dlv_path = item['path']
            rig_path = os.path.join(dlv_path, f"{asset_name}_rig_OK.ma")
            if os.path.exists(rig_path):
                MayaProcs.reference_scene(rig_path, asset_name)
                print(f"[postexecute] Referenced rig for {asset_name}")
            else:
                print(f"[postexecute] No rig found for {asset_name} at {rig_path}")

    cmds.file(rename=EXECUTED_FILE)
    cmds.file(save=True, type="mayaAscii", force=True)
    reroot_fbx(EXECUTED_FILE)

# ------------------------------------------------------
# CUSTOM
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

def read_json(json_path):
    """
    Lit un fichier JSON 
    """
    import json

    if not os.path.exists(json_path):
        print(f"[read_json] File not found: {json_path}")
        return {}

    with open(json_path, "r", encoding="utf-8") as f:
        data = json.load(f)

    return data

def remove_envvar_from_path(path):
    """
    Supprime la variable d'environnement du type ${DUCKPIPE_ROOT} dans un chemin
    chemin de type: "${DUCKPIPE_ROOT}/SPARK/Assets/Environments/KoreanTemple/dlv"
    """
    if '${DUCKPIPE_ROOT}' in path:
        env_var = "${DUCKPIPE_ROOT}"
        env_value = os.path.dirname(PROD_PATH)  # on remonte d'un cran pour avoir le DUCKPIPE_ROOT
        path = path.replace(env_var, env_value)
    return path

def get_asset_dependencies(asset_path):
    """
    Retourne la liste des dépendances d'un asset
    elles sont listées dans le fichier node.json dans le dossier de l'asset. sous refAssets
        {...
        "nodeInfos": {
            "refAssets": [
            "${DUCKPIPE_ROOT}/SPARK/Assets/Environments/KoreanTemple/dlv",
            "${DUCKPIPE_ROOT}/SPARK/Assets/Characters/Blue/dlv",
            "${DUCKPIPE_ROOT}/SPARK/Assets/Characters/Red/dlv"
            ]
            },...
        }   
    """
    asset_path = remove_envvar_from_path(asset_path)
    node_json_path = os.path.join(asset_path, "node.json")
    print(f"[get_asset_dependencies] Reading node.json: {node_json_path}")
    node_data = read_json(node_json_path)
    dependencies = []

    ref_assets = node_data.get("nodeInfos", {}).get("refAssets", [])
    for ref in ref_assets:
        ref = ref.replace("\\", "/")
        ref = remove_envvar_from_path(ref)
        parts = ref.split("/Assets/")
        if len(parts) < 2:
            continue
        asset_info = parts[1].split("/dlv")[0].split("/")
        if len(asset_info) < 2:
            continue
        asset_type = asset_info[0]
        asset_name = asset_info[1]
        dependencies.append({
            "type": asset_type,
            "name": asset_name,
            "path": ref
        })
    print(f"[get_asset_dependencies] Found {len(dependencies)} dependencies.")
    return dependencies


# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():
    preexecute()
    execute()
    postexecute()


if __name__ == "__main__":
    main()

