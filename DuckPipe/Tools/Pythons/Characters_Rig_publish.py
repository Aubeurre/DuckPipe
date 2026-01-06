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
    from Sub_Procs import Maya_surfacing_import_vp
    
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
        addAttr()

        if os.path.exists(os.path.join(dlv_path, "surfacing_export.json")):
            Maya_surfacing_import_vp.import_surf(os.path.join(dlv_path, "surfacing_export.json"))

        full_scene_path = os.path.join(dlv_path, file_name).replace("\\", "/")
        cmds.file(rename=full_scene_path)
        cmds.file(save=True, type="mayaAscii", prompt=False)

        createAssemble()
        addFacialToAssemble()


def postpublish():
    """
    Tout ce qui se passe ici se fait apres tout le reste une fois la scene fermee
    """
    print(" -> Post-publish")
    
    if IN_MAYA:
        cmds.file(save=True, type="mayaAscii")
        cmds.file(new=True, force=True)
        newFileName = file_name.replace(DEPT_SUFFIX, ASSEMBLESUFFIX)
        full_scene_path = os.path.join(dlv_path, newFileName)
        MayaProcs.sanitize_me(full_scene_path)
            
        
# ------------------------------------------------------
# Fonction MAYA
# ------------------------------------------------------

def addAttr():
    """
    ajout d un attr de animable or not for set assembly
    """
    if IN_MAYA:
        if not cmds.attributeQuery("asset_name", node="global_ctl", exists=True):
            cmds.addAttr("global_ctl", ln="asset_name", dt="string")
            cmds.setAttr("global_ctl.asset_name", e=1, keyable=0, channelBox=1)
            cmds.setAttr("global_ctl.asset_name" , asset_name, type='string')
            print(f"attribut name {asset_name} ajoute a global_ctl")

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
            
    print("[FACIAL]")
    def clean_facial() :
        # rangement
        if cmds.objExists("facial_def_jnt_grp"):
            cmds.parent("facial_def_jnt_grp", "DFJNT_GRP")
        if cmds.objExists("facial_geo_grp"):
            cmds.parent("facial_geo_grp", "ADDITIV_RIG")
        if cmds.objExists("facial_grp"):
            cmds.parent("facial_grp", "__RIG__")
        if cmds.objExists("Facial_ctl_set"):
            cmds.sets("Facial_ctl_set", e=1, fe="ctl_set")
            
        # connection des ctl HARD NAMING
        if cmds.objExists("center_HeadUp_ctl"):
            cmds.parentConstraint("center_HeadUp_ctl", "zone_01", mo=1)
            cmds.scaleConstraint("center_HeadUp_ctl", "zone_01", mo=1)
        if cmds.objExists("center_Skull_0_ctl"):
            cmds.parentConstraint("center_Skull_0_ctl", "zone_02", mo=1)
            cmds.scaleConstraint("center_Skull_0_ctl", "zone_02", mo=1)
        if cmds.objExists("center_Skull_1_ctl"):
            cmds.parentConstraint("center_Skull_1_ctl", "zone_03", mo=1)
            cmds.scaleConstraint("center_Skull_1_ctl", "zone_03", mo=1)
        if cmds.objExists("center_Skull_2_ctl"):
            cmds.parentConstraint("center_Skull_2_ctl", "zone_04", mo=1)
            cmds.scaleConstraint("center_Skull_2_ctl", "zone_04", mo=1)
        if cmds.objExists("center_Skull_3_ctl"):
            cmds.parentConstraint("center_Skull_3_ctl", "zone_05", mo=1)
            cmds.scaleConstraint("center_Skull_3_ctl", "zone_05", mo=1)
        if cmds.objExists("center_nose_2_fk_ctl"):
            cmds.parentConstraint("center_nose_2_fk_ctl", "c_nose_ctl_grp", mo=1)
            cmds.scaleConstraint("center_nose_2_fk_ctl", "c_nose_ctl_grp", mo=1)
        if cmds.objExists("center_eye_left_loc_grp"):
            cmds.pointConstraint("l_eyepocket_ctl", "center_eye_left_loc_grp", mo=1)
            cmds.scaleConstraint("l_eyepocket_ctl", "center_eye_left_loc_grp", mo=1)
        if cmds.objExists("center_eye_right_loc_grp"):
            cmds.pointConstraint("r_eyepocket_ctl", "center_eye_right_loc_grp", mo=1)
            cmds.scaleConstraint("r_eyepocket_ctl", "center_eye_right_loc_grp", mo=1)

        # BLINK
        if cmds.objExists("center_eye_left_globe_ctl.blink"):
            cmds.connectAttr("center_eye_left_globe_ctl.blink", "left_eyelid_curves_grp.Blink", f=1)
        if cmds.objExists("center_eye_left_globe_ctl.blink_pose"):
            cmds.connectAttr("center_eye_left_globe_ctl.blink_pose", "left_eyelid_curves_grp.Blink_pose", f=1)
        if cmds.objExists("center_eye_right_globe_ctl.blink"):
            cmds.connectAttr("center_eye_right_globe_ctl.blink", "right_eyelid_curves_grp.Blink", f=1)
        if cmds.objExists("center_eye_right_globe_ctl.blink_pose"):
            cmds.connectAttr("center_eye_right_globe_ctl.blink_pose", "right_eyelid_curves_grp.Blink_pose", f=1)
            
        # connection des shapes
        def replace_shape(aShape, bShape):    
            # --- Reconnecter les outMesh ---
            outMesh_conns = cmds.listConnections(aShape + ".outMesh", plugs=True, destination=True) or []
            for conn in outMesh_conns:
                cmds.disconnectAttr(aShape + ".outMesh", conn)
                cmds.connectAttr(bShape + ".outMesh", conn)
            
            # --- Reconnecter les worldMesh[0] ---
            worldMesh_conns = cmds.listConnections(aShape + ".worldMesh[0]", plugs=True, destination=True) or []
            for conn in worldMesh_conns:
                cmds.disconnectAttr(aShape + ".worldMesh[0]", conn)
                cmds.connectAttr(bShape + ".worldMesh[0]", conn)

        meshes = cmds.listRelatives("facial_geo_grp", allDescendents=True, type='mesh', fullPath=True) or []
        transforms = list(set(cmds.listRelatives(meshes, parent=True, fullPath=True))) if meshes else []

        for msh in transforms:
            dest = msh.replace("FACIAL_","").split("|")[-1]
            dest_orig = get_original_shape(dest)
            msh_shapes = cmds.listRelatives(msh, shapes=True, noIntermediate=True, fullPath=True) or []
            
            replace_shape(dest_orig, msh_shapes[-1])
            print("RECONNECT:", dest_orig, msh_shapes[-1])

        # DEBUG pout les stickys faut prendre l orig au niv.....
        orig = get_original_shape("body")
        cmds.connectAttr(f"{orig}.outMesh", "center_lips_lvl1_surf_pWrap.drivers[0].driverBindGeometry", f=1)
        cmds.connectAttr(f"{orig}.outMesh", "center_eyebrow_lvl1_surf_pWrap.drivers[0].driverBindGeometry", f=1)

    def get_original_shape(mesh):
        # Trouve les shapes associees
        shapes = cmds.listRelatives(mesh, shapes=True, fullPath=True) or []
        # Recupere l historique complete du mesh
        history = cmds.listHistory(shapes[0]) or []
        all_shapes = [h for h in history if cmds.nodeType(h) == "mesh"]
        for shape in all_shapes:
            connection = cmds.listConnections(f"{shape}.inMesh")
            if connection == None:
                return shape  # fallback
            
    facial_file_path = EXECUTED_FILE.replace(file_name, file_name.replace(DEPT_SUFFIX, FACIALSUFFIX))

    if os.path.exists(facial_file_path):
        MayaProcs.sanitize_ma(facial_file_path)
        print(f"Ajout du facial: {facial_file_path}")
        cmds.file(facial_file_path, i=True, type="mayaAscii",
                    ignoreVersion=True, ra=True,
                    mergeNamespacesOnClash=False,
                    namespace=":", options="v=0")
        clean_facial()
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
