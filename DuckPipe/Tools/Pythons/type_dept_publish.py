"""
Publish pour BLENDER et MAYA
"""

import os
import sys


TRASHLIST = ['TRASH']

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

    if IN_MAYA:
        # des procs dans maya
        pass

    elif IN_BLENDER:
        # des procs dans blender
        pass



def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste
    """
    print("Post-publish")

    if IN_MAYA:
        MayaProcs.clean_publish(TRASHLIST)

        cmds.file(save=True, type="mayaAscii")

    elif IN_BLENDER:
        BlenderProcs.clean_publish(TRASHLIST)

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
