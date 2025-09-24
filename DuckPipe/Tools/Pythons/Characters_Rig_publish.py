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


RIGSUFFIX = "_rig_OK"
FACIALSUFFIX = "_facial_OK"
ASSEMBLESUFFIX = "_assemble_OK"

# ------------------------------------------------------
# Detection environnement
# ------------------------------------------------------
IN_MAYA = False

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
        MayaProcs.import_ref()
        MayaProcs.clean_publish(['__TRASH__', '__UTILS__', '__REF__'])
        addShading()

        cmds.file(save=True, type="mayaAscii")


def postpublish(dlv_path, asset_name):
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print(" -> Post-publish")
    
    facial_file_name = file_name.replace(RIGSUFFIX, FACIALSUFFIX)
    facial_file_path = EXECUTED_FILE.replace(file_name, facial_file_name)

    if IN_MAYA:
        createAssemble(file_name, EXECUTED_FILE)
        addFacialToAssemble(EXECUTED_FILE, facial_file_path)

        cmds.file(save=True, type="mayaAscii")

        # on split tous les costumes
        costumes_list = ["casual"]
        for costumes in costumes_list:
            newFileName = f"{asset_name}_{costumes}{RIGSUFFIX}"
            newFilePath = os.path.join(dlv_path, newFileName).replace("\\", "/")
            print(f"Creation assemble: {newFilePath}")
            cmds.file(rename=newFilePath)
            cmds.file(save=True, type="mayaAscii")
            

        
# ------------------------------------------------------
# Fonction MAYA
# ------------------------------------------------------
def addShading():
    """
    on ajoute les shaders pour l'anim
    """
    if IN_MAYA:
        pass
    pass


def createAssemble(file_name, file_path):
    """
    creation de la scene d'Assemble pour le rig et le facial'
    """
    if IN_MAYA:
        newFileName = file_name.replace(RIGSUFFIX, ASSEMBLESUFFIX)
        newFilePath = file_path.replace(file_name, newFileName)
        print(f"Creation assemble: {newFilePath}")
        cmds.file(rename=newFilePath)
        cmds.file(save=True, type="mayaAscii")


def addFacialToAssemble(published_file, facial_file_path):
    """
    Importation du facial et connection
    """
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


# --- MAIN---
file_name = os.path.basename(EXECUTED_FILE)
file_root, file_ext = os.path.splitext(file_name)
asset_path = os.path.dirname(os.path.dirname(os.path.dirname(EXECUTED_FILE)))
dlv_path = os.path.join(asset_path, "dlv")
asset_name = file_root.replace(RIGSUFFIX, "")
studio_dlv_path = dlv_path.replace("\\", "/").replace(LOCAL_PATH, PROD_PATH)

def main():        
    prepublish()
    publish()
    postpublish(dlv_path, asset_name)

if __name__ == "__main__":
    main()
