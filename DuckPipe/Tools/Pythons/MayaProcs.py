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


def reset_scene():
    """
    Reset la scene
    """
    cmds.file(new=True, force=True)


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
def loc_to_group(loc): 
    """ 
    Convertir un locator en group maya 
    """ 
    # Ceation du groupe 
    print(loc)
    grp_name = loc.replace("_GRP", "") 
    if not cmds.objExists(grp_name): 
        grp = cmds.group(em=True, name=grp_name) 
        pos = cmds.xform(loc, q=True, ws=True, t=True) 
        cmds.xform(grp, ws=True, t=pos) 

    # Parentage des enfants 
    children = cmds.listRelatives(loc, c=True, f=False) or [] 
    for child in children: 
        if not child == cmds.listRelatives(loc, shapes=True)[0]: 
            print(child) 
            cmds.parent(child, grp_name) 

    # parentage au parent 
    
    parent_list = cmds.listRelatives(loc, p=True, f=False) or []
    for nodeparent in parent_list: 
        parent_transform = nodeparent[0]
        grp_name_parent = nodeparent.replace("_GRP", "") 
        print("is parent:", nodeparent) 
        if not cmds.objExists(grp_name_parent): 
            loc_to_group(nodeparent) 
        cmds.parent(grp_name, grp_name_parent) 

    # Suppression du locator 
    cmds.delete(loc)

def confo_from_maya():
    locList = cmds.ls(exactType="locator", l=False) or []
    for locShape in locList:
        loc = cmds.listRelatives(locShape, p=True, f=False)[0]
        if loc.endswith("_GRP"):
            loc_to_group(loc)
    print("Maya Groups done !")