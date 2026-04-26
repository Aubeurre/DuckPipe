# -*- coding: utf-8 -*-
import maya.cmds as cmds
import os
import maya.mel as mel

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


def reference_fbx(file_path, parent_grp= "__REF__"):

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
            

def assign_basic_material_to_ref(ref_grp):
    """
    Cree et assigne un materiau de base a une reference
    """

    # Cree un materiau lambert de base
    material_name = "ducky_MAT"
    material = cmds.shadingNode('lambert', asShader=True, name=material_name)
    cmds.setAttr(f"{material}.color", 0.8, 0.8, 0.8, type="double3")

    # Cree un shading group
    shading_group = f"{material}_SG"
    shading_group = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name=shading_group)
    cmds.connectAttr(f"{material}.outColor", f"{shading_group}.surfaceShader", force=True)

    
    if not cmds.objExists(ref_grp):
        print(f"[assign_basic_material_to_ref] Reference group '{ref_grp}' does not exist.")
        return
    
    # Assigne le materiau a tous les meshes sous le groupe de reference
    meshes = cmds.listRelatives(ref_grp, allDescendents=True, type='mesh') or []
    for mesh in meshes:
        transform = cmds.listRelatives(mesh, parent=True, fullPath=True)[0]
        cmds.sets(transform, e=True, forceElement=shading_group)

    print(f"[assign_basic_material_to_ref] Assigned basic material to reference group '{ref_grp}'.")


def reference_scene(file_path, asset_name, load=True):

    if not os.path.exists(file_path):
        cmds.warning(f"File not found: {file_path}")
        return None

    ns = asset_name

    if load:
        cmds.file(
            file_path,
            reference=True,
            namespace=ns,
            mergeNamespacesOnClash=False
        )
    else:
        cmds.file(
            file_path,
            reference=True,
            namespace=ns,
            mergeNamespacesOnClash=False,
            loadReferenceDepth="none"
        )
        print(f"[deferred] injected unloaded Scene: {asset_name}")
        return None


def inject_reference_into_ma(ma_path, ref_path, namespace):
    print('run inject_reference_into_ma on:', ma_path, ref_path, namespace)
    
    ref_path = ref_path.replace("\\", "/")

    ref_block = [
        f'file -rdi 1 -ns "{namespace}" -dr 1 "{ref_path}";\n',
        f'file -r -ns "{namespace}" "{ref_path}";\n'
    ]

    with open(ma_path, "r", encoding="utf-8") as f:
        lines = f.readlines()

    # Evite doublon
    if any(ref_path in l for l in lines):
        print(f"[inject] already exists: {namespace}")
        return

    # Trouver dernier fileInfo
    insert_index = 0
    for i, line in enumerate(lines):
        if line.startswith("fileInfo"):
            insert_index = i + 1

    # Injection
    lines.insert(insert_index, "\n// DuckPipe injected reference\n")
    for l in reversed(ref_block):
        lines.insert(insert_index + 1, l)

    tmp = ma_path + ".tmp"
    with open(tmp, "w", encoding="utf-8") as f:
        f.writelines(lines)

    os.replace(tmp, ma_path)

    print(f"[inject] OK: {ref_path}")


def fbx_to_gpu_cache(fbx_path):
    if not os.path.exists(fbx_path):
        raise RuntimeError("FBX introuvable")
    print('go', fbx_path)

    base_dir = os.path.dirname(fbx_path)
    base_name = os.path.splitext(os.path.basename(fbx_path))[0]

    # Plugins
    if not cmds.pluginInfo("fbxmaya", q=True, loaded=True):
        cmds.loadPlugin("fbxmaya")
    if not cmds.pluginInfo("gpuCache", q=True, loaded=True):
        cmds.loadPlugin("gpuCache")

    # Nodes existants avant import
    nodes_before = set(cmds.ls(long=True))

    # Import FBX
    cmds.file(fbx_path, i=True, type="FBX", ignoreVersion=True, mergeNamespacesOnClash=False)

    # Nodes après import
    nodes_after = set(cmds.ls(long=True))
    imported_nodes = nodes_after - nodes_before

    # Sélectionne tous les meshes importés
    new_meshes = [n for n in imported_nodes if cmds.nodeType(n) == "mesh"]
    if not new_meshes:
        raise RuntimeError("Aucun mesh trouvé après import FBX")

    # Récupère les transforms parents uniques
    transforms = list(set(cmds.listRelatives(new_meshes, parent=True, fullPath=True)))

    # Supprime les transforms vides ou n'ayant pas de mesh enfant
    clean_transforms = []
    for t in transforms:
        children = cmds.listRelatives(t, children=True) or []
        if any(cmds.nodeType(c) == "mesh" for c in children):
            clean_transforms.append(t)
    if not clean_transforms:
        raise RuntimeError("Aucun transform valide pour GPU cache")

    # Crée le GPU cache et récupère le path exact
    gpu_paths = cmds.gpuCache(
        clean_transforms,
        startTime=1,
        endTime=1,
        optimize=True,
        writeMaterials=False,
        dataFormat="ogawa",
        directory=base_dir,
        fileName=base_name
    )
    gpu_path = gpu_paths[0]

    # Supprime les meshes FBX
    cmds.delete(clean_transforms)

    # Crée un node GPU cache qui pointe sur le fichier exact
    
    parent_transform = cmds.createNode("transform", name=base_name + "_gpu")
    gpu_node = cmds.createNode("gpuCache", name=base_name + "_gpuShape", parent=parent_transform)
    cmds.setAttr(gpu_node + ".cacheFileName", gpu_path, type="string")


    print("GPU cache affiché :", gpu_node)
    print("Chemin exact :", gpu_path)
    return parent_transform, gpu_path


def apply_transform(node, pos, rot, scale):
    print(node, pos, rot, scale)
    cmds.xform(node, ws=True, t=pos)
    cmds.xform(node, ws=True, ro=rot)
    cmds.xform(node, ws=True, s=scale)


def cleanReferencesBeforeSave(env_name="DUCKPIPE_ROOT"):

    env_value = os.environ.get(env_name)
    if not env_value:
        cmds.warning(f"{env_name} not found. Skipping clean.")
        return

    env_value = os.path.normpath(env_value)

    references = cmds.ls(type="reference")

    for ref in references:
        try:
            ref_path = cmds.referenceQuery(ref, filename=True)
        except:
            continue

        if not ref_path:
            continue

        normalized_path = os.path.normpath(ref_path)

        if normalized_path.startswith(env_value):
            new_path = normalized_path.replace(
                env_value,
                f"${{{env_name}}}"
            )

            try:
                cmds.file(new_path, loadReference=ref)
                print(f"[CLEANED] {ref}")
            except Exception as e:
                print(f"[ERROR] {ref} -> {e}")


def import_fbx(file_path):
    cmds.loadPlugin("fbxmaya", quiet=True)
    mel.eval(f'FBXImport -f "{file_path}"')


def deal_with_cinecam(cam_path, shotname):
    def create_image_plane_for_camera(cam, image_path=""):
        shapes = cmds.listRelatives(cam, shapes=True)
        camera_shape = shapes[0]
        img_plane, img_plane_shape = cmds.imagePlane(camera=camera_shape, n=f'{cam}_imageplane')

        if image_path:
            cmds.setAttr(img_plane_shape + ".imageName", image_path, type="string")
    import_fbx(cam_path)

    oldName = "cineCam"
    new_name = f'{shotname}_cam'
    objs = cmds.ls("*cineCam*", long=False)

    for i, obj in enumerate(objs):
        short_name = obj.split("|")[-1]
        short_new_name = short_name.replace(oldName, new_name)
        try:
            cmds.rename(obj, short_new_name)
        except:
            pass
    
    create_image_plane_for_camera(f'{new_name}_cam', '')
        
    return f'{new_name}'


def create_shots_sequencer(shotlist, seqname, cam_path):

    blank = 0
    margingframein = 0
    for item in shotlist:

        # gestion camera
        shotname = f"{seqname}_{item['name']}"
        camebasename = deal_with_cinecam(cam_path, shotname)
        try:
            cmds.parent(f'{camebasename}_grp', 'CAM')
        except:
            print('CAN NOT PARENT', f'{camebasename}_grp', 'CAM')
                
        # gestion shot sequencer
        shot_total_frame = int(item['outframe']) - int(item['inframe'])

        # get shot time to know if g ofor a 100 or 200 or more marging
        blank = blank + ((shot_total_frame//100)+1)*100
        cmds.shot(sn = item['name'], 
                st = 1 + blank,
                et = shot_total_frame + blank,
                sst = margingframein, 
                set = margingframein + shot_total_frame,
                currentCamera = f'{camebasename}_cam')
        margingframein += shot_total_frame + 100