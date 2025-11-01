"""
Publish pour MAYA (le rig se fera toujours dans maya avec DuckPipe)

0-
lance le script de publish rig custom
1-
Delete trash, remove references, 
2-
ajouter les shaders, sauvegarder le rig OK
3-
ajouter le facial si il existe, faire les connections, sauvegarder l'assemble OK
4- 
on split tous les costumes
5-
on ajoute une ref vers le split casual dans une scene vide, sauvegarder le Actor OK

"""

import os
import sys

# ------------------------------------------------------
# Constantes
# ------------------------------------------------------

DEPT_SUFFIX = "_rig_OK"
FACIALSUFFIX = "_facial_OK"
ASSEMBLESUFFIX = "_assemble_OK"
LAYOUTSUFFIX = "_layout_OK"

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

    import BlenderProcs
    import GlobalProcs
    import import_surfacing

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
        # clean la scene
        MayaProcs.remove_ref()
        MayaProcs.clean_publish(['__TRASH__', '__UTILS__', '__REF__'])
        cmds.group("__RIG__", "MODEL_OK", n=asset_name)

        # injecte le surfacing (meme si a l anim on veut pas utiliser ce shading.....)
        surfacing_export_json = os.path.join(dlv_path, "surfacing_export.json")
        import_surfacing.import_surfacing(surfacing_export_json)

        # save la scene
        full_scene_path = os.path.join(dlv_path, file_name).replace("\\", "/")
        cmds.file(rename=full_scene_path)
        cmds.file(save=True, type="mayaAscii", prompt=False)


def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-publish")
    

    if IN_MAYA:
        # createRigLayout() # fonctionne pas encore avec le batch car change de scene.....

        # on ajoute le facial
        addFacialToAssemble()

        # save la scene
        cmds.file(save=True, type="mayaAscii")

        # on fait ca plus tard donc je com le tout
        '''
        # on split tous les costumes
        costumes_list = ["casual"]
        for costumes in costumes_list:
            newFileName = f"{asset_name}_{costumes}{DEPT_SUFFIX}"
            newFilePath = os.path.join(dlv_path, newFileName).replace("\\", "/")
            print(f"Creation assemble: {newFilePath}")
            cmds.file(rename=newFilePath)
            cmds.file(save=True, type="mayaAscii")
        '''

        createAssemble()
            
        
# ------------------------------------------------------
# Fonction MAYA
# ------------------------------------------------------

def createRigLayout():
    """
    creation d un rig simplifie pour le layout'
    """
    if IN_MAYA:
        newFileName = file_name.replace(DEPT_SUFFIX, LAYOUTSUFFIX)
        full_scene_path = os.path.join(dlv_path, newFileName)
        print(f"Creation layout: {full_scene_path}")
        cmds.file(rename=full_scene_path)
        cmds.file(save=True, type="mayaAscii")


def createAssemble():
    """
    creation de la scene d'Assemble pour le rig et le facial'
    """
    if IN_MAYA:
        newFileName = file_name.replace(DEPT_SUFFIX, ASSEMBLESUFFIX)
        full_scene_path = os.path.join(dlv_path, newFileName)
        print(f"Creation assemble: {full_scene_path}")
        cmds.file(rename=full_scene_path)
        cmds.file(save=True, type="mayaAscii")


def addFacialToAssemble():
    """
    Importation du facial et connection
    """
    facial_file_name = file_name.replace(DEPT_SUFFIX, FACIALSUFFIX)
    facial_file_path = EXECUTED_FILE.replace(file_name, facial_file_name)

    if IN_MAYA:
        if os.path.exists(facial_file_path):
            print(f"Ajout du facial: {facial_file_path}")
            cmds.file(facial_file_path, i=True, type="mayaAscii",
                      ignoreVersion=True, ra=True,
                      mergeNamespacesOnClash=False,
                      namespace=":", options="v=0")
            # TODO: connections
        else:
            print("Pas de facial trouve.")


# ------------------------------------------------------
# Main
# ------------------------------------------------------
def main():        
    prepublish()
    publish()
    postpublish()

if __name__ == "__main__":
    main()
