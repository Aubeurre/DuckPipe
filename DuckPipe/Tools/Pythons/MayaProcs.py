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
    cmds.file(path, i=1)


def export_hierarchy_fbx(root_name, filepath):
    """
    Exporte FBX
    """
    if not cmds.objExists(root_name):
        cmds.error(f" Objet '{root_name}' introuvable")
        return
    
    # get all hierarchy
    descendants = cmds.listRelatives(root_name, allDescendents=True, fullPath=True) or []
    to_export = [root_name] + descendants

    cmds.select(to_export, replace=True)

    # Folder
    dirpath = os.path.dirname(filepath)
    if not os.path.exists(dirpath):
        os.makedirs(dirpath)

    # Export FBX (via plugin FBX)
    try:
        import maya.mel as mel
        mel.eval('FBXResetExport;')  # reset settings
        mel.eval('FBXExportBakeComplexAnimation -v false;')
        mel.eval('FBXExportInputConnections -v false;')
        mel.eval(f'FBXExport -f "{filepath}" -s;')
    except Exception as e:
        cmds.error(f" Erreur export FBX : {e}")
        return

    print(f" Export FBX Done : {filepath}")
    

def reference_fbx(file_path, parent_grp):
    """reference un FBX et le parent a parent_grp"""
    if os.path.exists(file_path):
        ref_node = cmds.file(file_path, r=True, type="FBX", namespace=":")
        ref_nodes = cmds.referenceQuery(ref_node, nodes=True, dagPath=True) or []
        root_nodes = [n for n in ref_nodes if not cmds.listRelatives(n, parent=True)]
        if root_nodes:
            cmds.parent(root_nodes, parent_grp)
        print(f"Import FBX : {file_path}")
    else:
        #  (fichier manquant)
        cmds.file(file_path, r=True, type="FBX", namespace=":", deferReference=True)
        print(f"Fichier manquant : {file_path}")
        

# ------------------------------------------------------
# Fonction BLENDER to MAYA
# ------------------------------------------------------
def confo_from_maya():
    locList = cmds.ls(exactType="locator", l=True) or []
    all_ref = get_all_ref_items()
    for locShape in locList:
        # on ne touche pas au refs 
        if not locShape in all_ref: 
            loc = cmds.listRelatives(locShape, p=True, f=True)[0]
            if loc.endswith("_GRP"):
                print("->", locShape)
                cmds.delete(locShape)
    print("Maya Groups done !")
    