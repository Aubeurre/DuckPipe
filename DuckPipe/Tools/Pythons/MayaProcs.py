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
    path = path.replace("\\", "/")
    if os.path.exists(path):
        cmds.file(path, i=1)


def export_hierarchy_by_name(root_name, filepath):
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
    import maya.mel as mel
    filepath = filepath.replace("\\", "/")

    folder = os.path.dirname(filepath)
    if not os.path.exists(folder):
        os.makedirs(folder)

    if not cmds.pluginInfo("fbxmaya", q=True, loaded=True):
        cmds.loadPlugin("fbxmaya")

    objs = cmds.ls(selection=True)
    if not objs:
        pass

    cmds.select(objs, r=True)

    mel.eval('FBXResetExport;')
    mel.eval('FBXExportBakeComplexAnimation -v false;')
    mel.eval('FBXExportInputConnections -v false;')
    mel.eval('FBXExportFileVersion -v FBX202000;')

    # Export FBX
    mel.eval(f'FBXExport -f "{filepath}" -s;')
    print(f" FBX exporte : {filepath}")


# NE FONCTIONNE PAS EN BATCH MAIS OUI DANS LE GUI
def reference_fbx(file_path, parent_grp):
    """Reference un FBX et le parent a parent_grp"""
    if os.path.exists(file_path):
        file_path = file_path.replace("\\", "/")

        # plugin FBX
        if not cmds.pluginInfo("fbxmaya", q=True, loaded=True):
            try:
                cmds.loadPlugin("fbxmaya")
            except Exception as e:
                cmds.error(f"Impossible de charger fbxmaya : {e}")

        try:
            print(f"Import FBX : {file_path}")
            ref_node = cmds.file(file_path, r=True, type="FBX", ignoreVersion=True, options="v=0;", namespace="fbxRef")
            ref_nodes = cmds.referenceQuery(ref_node, nodes=True, dagPath=True) or []
            root_nodes = [n for n in ref_nodes if not cmds.listRelatives(n, parent=True)]
            if root_nodes:
               cmds.parent(root_nodes, parent_grp)
        except Exception as e:
            cmds.error(f"Erreur reference FBX : {e}")

    else:
        try:
            cmds.file(file_path, r=True, type="FBX", namespace=":", ignoreVersion=True, options="v=0;", deferReference=True)
            print(f"[WARN] Fichier manquant (reference differee) : {file_path}")
        except Exception as e:
            cmds.error(f"Erreur reference FBX manquant : {e}")

        

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
    