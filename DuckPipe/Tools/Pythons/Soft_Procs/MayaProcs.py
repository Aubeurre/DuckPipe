# -*- coding: utf-8 -*-
import maya.cmds as cmds
import os

def import_ref():
    """
    Importe toutes les references et supprime les namespaces
    """

    for ref in cmds.ls(type='reference'):
        if ref == 'sharedReferenceNode':
            pass
        else:
            try:
                rFile = cmds.referenceQuery(ref, f=True)
                cmds.file(rFile, importReference=True)
            except:
                pass
                
    cmds.namespace(setNamespace=':')
    all_namespaces = [x for x in cmds.namespaceInfo(listOnlyNamespaces=True, recurse=True) if x != "UI" and x != "shared"]
    if all_namespaces:
        all_namespaces.sort(key=len, reverse=True)
        for namespace in all_namespaces:
            if cmds.namespace(exists=namespace) is True:
                cmds.namespace(removeNamespace=namespace, mergeNamespaceWithRoot=True)

    
def sanitize_ma(path):
    with open(path, "r", encoding="utf-8") as f:
        lines = f.readlines()

    with open(path, "w", encoding="utf-8") as f:
        for line in lines:
            if ".oclr" not in line:
                f.write(line)


def remove_ref():
    """
    remove al ref in scene
    """
    for ref in cmds.ls(type='reference'):
        if ref == 'sharedReferenceNode':
            pass
        else:
            try:
                rFile = cmds.referenceQuery(ref, f=True)
                cmds.file(rFile, rr=True)
            except:
                pass


def clean_publish(listToDelete):
    """
    Supprime les objets de la liste s'ils existent
    """
    for item in listToDelete:
        if cmds.objExists(item):
            cmds.delete(item)


def reset_scene(path):
    """
    Reset la scene avec le template
    """
    cmds.file(new=True, force=True)
    path = path.replace("\\", "/")
    if os.path.exists(path):
        cmds.file(path, i=1)


def export_hierarchy_by_name(root_names, filepath):
    """
    Exporte en un seul fichier FBX plusieurs racines (EMPTY, GROUP, JOINT, MESH, etc.) 
    ainsi que toutes leurs hierarchies (descendants inclus).
    Compatible pipeline  minimal, propre et fiable.
    """
    # Selection vide
    cmds.select(clear=True)

    to_export = []

    for root in root_names:
        if not cmds.objExists(root):
            print(f"Objet '{root}' introuvable  ignore")
            continue

        # Ajouter racine
        to_export.append(root)

        # Ajouter tous les descendants
        descendants = cmds.listRelatives(root, allDescendents=True, fullPath=True) or []
        to_export.extend(descendants)

    if not to_export:
        cmds.error(" Aucun objet valide trouve  export annule.")
        return

    # Selection globale
    cmds.select(list(set(to_export)), replace=True)  # set() pour eviter les doublons

    # Creer dossier destination si besoin
    folder = os.path.dirname(filepath.replace("\\", "/"))
    if not os.path.exists(folder):
        os.makedirs(folder)

    # Charger plugin FBX si necessaire
    if not cmds.pluginInfo("fbxmaya", q=True, loaded=True):
        cmds.loadPlugin("fbxmaya")

    import maya.mel as mel

    # Reset + options export FBX pipeline-friendly
    mel.eval('FBXResetExport;')
    mel.eval('FBXExportBakeComplexAnimation -v false;')
    mel.eval('FBXExportInputConnections -v false;')
    mel.eval('FBXExportFileVersion -v FBX202000;')

    # Export FBX
    filepath = filepath.replace("\\", "/")
    print(filepath)
    print(root_names)
    mel.eval(f'FBXExport -f "{filepath}" -s;')

    print(f"FBX exporte avec : {root_names} - {filepath}")


def reroot_fbx(scene_path):

    print("REROOT:", scene_path)
    print("Existe ?", os.path.exists(scene_path))

    with open(scene_path, "r", encoding="utf-8") as f:
        print("lecture")
        lines = f.readlines()

    with open(scene_path, "w", encoding="utf-8") as f:
        print("ecriture")
        for line in lines:
            if "__dummy" in line:
                f.write(line.replace("__dummy", ""))
                print(line.replace("__dummy", ""))
            else:
                f.write(line)
        f.flush()
        os.fsync(f.fileno())

    print("REROOT DONE.")

# NE FONCTIONNE PAS EN BATCH MAIS OUI DANS LE GUI
def reference_fbx(file_path, parent_grp):

    file_path = file_path.replace("\\", "/")

    if not cmds.pluginInfo("fbxmaya", q=True, loaded=True):
        try:
            cmds.loadPlugin("fbxmaya")
        except Exception as e:
            cmds.error(f"Impossible de charger fbxmaya : {e}")

    if not cmds.objExists(parent_grp):
        cmds.group(em=1, n=parent_grp)

    real_file_exists = os.path.exists(file_path)

    # on cee le fbx qui pointe bien comme ca ne va pas charger et donc crash
    if not real_file_exists:
        print(f"[WARN] Fichier inexistant creation du fbx : {file_path}")
        ref_node = cmds.file(file_path,
                             r=True,
                             type="FBX",
                             ignoreVersion=True,
                             options="v=0;",
                             namespace=":")
        print(f"[OK]")

    else:
        dummypath = file_path.replace(".fbx", "__dummy.fbx")
        print(f"[WARN] Fichier existant creation du fbx : {dummypath}")
        ref_node = cmds.file(dummypath,
                             r=True,
                             type="FBX",
                             ignoreVersion=True,
                             options="v=0;",
                             namespace=":")
        print(f"[DUMY OK]")
        # on reroot apres en ecriture car maya en batch ne veut pas le faire.
            
    