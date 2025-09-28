# -*- coding: utf-8 -*-
import bpy
import os
    
coll_to_empty = {}


def reference_fbx(file_path, parent_grp_name="REF"):
    """
    Importe un FBX et le parent à parent_grp_name (collection ou objet vide)
    """
    if not os.path.exists(file_path):
        print(f"Fichier manquant : {file_path}")
        return

    # créer ou récupérer la collection cible
    if parent_grp_name in bpy.data.collections:
        parent_grp = bpy.data.collections[parent_grp_name]
    else:
        parent_grp = bpy.data.collections.new(parent_grp_name)
        bpy.context.scene.collection.children.link(parent_grp)

    # importer le FBX
    bpy.ops.import_scene.fbx(filepath=file_path)
    imported_objects = bpy.context.selected_objects

    for obj in imported_objects:
        # unlink de toutes les collections existantes
        for col in obj.users_collection:
            col.objects.unlink(obj)
        # link dans la collection cible
        parent_grp.objects.link(obj)

    print(f"Import FBX : {file_path} → {parent_grp_name}")


def clean_publish(listToDelete):
    """
    Supprime les objets de la liste s ils existent
    """
    for name in listToDelete:
        coll = bpy.data.collections.get(name)
        if coll:
            for obj in list(coll.objects):
                bpy.data.objects.remove(obj, do_unlink=True)
            for sub in list(coll.children):
                bpy.data.collections.remove(sub)
            bpy.data.collections.remove(coll)
    print("clean_publish Done")


def export_hierarchy_by_name(empty_name, filepath):
    """
    Exporte en FBX un Empty et tous ses descendants.
    """
    # Get
    obj = bpy.data.objects.get(empty_name)
    if not obj:
        print(f"Aucun objet trouve avec le nom : {empty_name}")
        return
    
    if obj.type != 'EMPTY':
        print(f"L'objet '{empty_name}' n est pas un Empty")
        return

    bpy.ops.object.select_all(action='DESELECT')

    # Selection 
    def select_hierarchy(o):
        o.select_set(True)
        for child in o.children:
            select_hierarchy(child)

    select_hierarchy(obj)
    bpy.context.view_layer.objects.active = obj

    # Export FBX
    bpy.ops.export_scene.fbx(
        filepath=filepath,
        use_selection=True,
        apply_unit_scale=True,
        bake_space_transform=False,
        object_types={'EMPTY', 'MESH', 'ARMATURE'},  # ajoute d'autres types si besoin
        apply_scale_options='FBX_SCALE_UNITS'
    )

    print(f" Export FBX Done : {filepath}")


def reset_scene(path):
    """
    Reset la scène avec le template Blender
    """
    bpy.ops.wm.read_homefile(use_empty=True)
    bpy.ops.wm.open_mainfile(filepath=path)


# ------------------------------------------------------
# Fonction BLENDER to MAYA
# ------------------------------------------------------

def collection_to_empty(coll, parent_empty=None):
    """
    Cree un Empty pour representer la collection
    """
    empty_name = coll.name + "_GRP"

    # check empty
    if empty_name in bpy.data.objects:
        empty = bpy.data.objects[empty_name]
    else:
        empty = bpy.data.objects.new(empty_name, None)
        bpy.context.scene.collection.objects.link(empty)

    # Parent
    if parent_empty:
        empty.parent = parent_empty

    # childs
    for obj in coll.objects:
        obj.parent = empty

    coll_to_empty[coll] = empty

    # Recurse
    for sub_coll in coll.children:
        collection_to_empty(sub_coll, empty)
        

def confo_from_blender():
    """
    Remplace les collections par des Empties
    """
    for coll in bpy.data.collections:
        if not coll.library:
            collection_to_empty(coll)
    print("Blender Empties done !")