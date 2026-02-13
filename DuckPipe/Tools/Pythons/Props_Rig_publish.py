"""
Publish pour MAYA (le rig se fera toujours dans maya avec DuckPipe)

0-
lance le script de publish rig custom
1-
Delete trash, remove references, 
2-
ajouter les shaders, sauvegarder le rig OK
"""

import os
import sys
import shutil

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------

DEPT_SUFFIX = "_rig_OK"

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
    from Sub_Procs import Surfacing_import
    
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
    print(" -> Pre-publish")
        
    if IN_MAYA:  
        # run custom script if exists
        rig_scene_folder = os.path.dirname(EXECUTED_FILE)
        custom_script_path = os.path.join(rig_scene_folder, "customScripts.py")
        print("custom_script_path:", custom_script_path)

        if rig_scene_folder not in sys.path:
            sys.path.append(rig_scene_folder)

        if os.path.exists(custom_script_path):
            import customScripts
            customScripts.execute()


def publish():
    """
    Tout ce qui se passe ici se fait dans la scene de OK
    """
    print(" -> publish")
    
    if IN_MAYA:
        MayaProcs.remove_ref()
        MayaProcs.clean_publish(['__TRASH__', '__UTILS__', '__REF__'])
        cmds.group("__RIG__", "MODEL_OK", n=asset_name)

        if os.path.exists(os.path.join(dlv_path, "surfacing_export.json")):
            Surfacing_import.import_surf(os.path.join(dlv_path, "surfacing_export.json"))

        full_scene_path = os.path.join(dlv_path, file_name).replace("\\", "/")
        cmds.file(rename=full_scene_path)
        cmds.file(save=True, type="mayaAscii", prompt=False)

        addAssemblyAttr()


def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste une fois la scene fermee
    """
    print(" -> Post-publish")
    
    if IN_MAYA:
        cmds.file(save=True, type="mayaAscii")
        cmds.file(new=True, force=True)
        server_dlv_path = os.path.join(studio_dlv_path, file_name).replace("\\", "/")
        shutil.copy2(EXECUTED_FILE, server_dlv_path)
        print(f"[postpublish] Copied {EXECUTED_FILE} -> {server_dlv_path}")
            
        
# ------------------------------------------------------
# Fonction MAYA
# ------------------------------------------------------

def addAssemblyAttr():
    """
    ajout d un attr de animable or not for set assembly
    """
    if IN_MAYA:
        if cmds.objExists("global_ctl"):
            if not cmds.attributeQuery("is_assembly_animable", node="global_ctl", exists=True):
                cmds.addAttr("global_ctl", ln="is_assembly_animable", at="bool", dv=1)
                cmds.setAttr("global_ctl.is_assembly_animable", e=1, keyable=0, channelBox=1)
                print("attribut is_assembly_animable ajoute a global_ctl")
            if not cmds.attributeQuery("asset_name", node="global_ctl", exists=True):
                cmds.addAttr("global_ctl", ln="asset_name", dt="string")
                cmds.setAttr("global_ctl.asset_name", e=1, keyable=0, channelBox=1)
                cmds.setAttr("global_ctl.asset_name" , asset_name, type='string')
                print(f"attribut name {asset_name} ajoute a global_ctl")


# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():       
    prepublish()
    publish()
    postpublish()

if __name__ == "__main__":
    main()
