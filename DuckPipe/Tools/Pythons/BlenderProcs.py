# -*- coding: utf-8 -*-
import bpy
import os
    
coll_to_empty = {}

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