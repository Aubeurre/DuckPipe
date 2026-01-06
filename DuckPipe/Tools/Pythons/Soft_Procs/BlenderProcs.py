# -*- coding: utf-8 -*-
import bpy
import os
from pathlib import Path

def assign_basic_material_to_all():
    """
    Assigne un materiau basique a tous les objets importes
    """
    basic_mat_name = "Ducky_MAT"
    if basic_mat_name in bpy.data.materials:
        basic_mat = bpy.data.materials[basic_mat_name]
    else:
        basic_mat = bpy.data.materials.new(name=basic_mat_name)
        basic_mat.diffuse_color = (0.8, 0.8, 0.8, 1)

    for obj in bpy.context.selected_objects:
        if obj.type == 'MESH':
            if len(obj.data.materials) == 0:
                obj.data.materials.append(basic_mat)
            else:
                for i in range(len(obj.data.materials)):
                    obj.data.materials[i] = basic_mat

    print("Assigned basic material to all imported objects.")

def reference_fbx(file_path, parent_grp_name="REF"):
    """
    Importe un FBX et le parent a parent_grp_name (collection ou objet vide)
    """
    if not os.path.exists(file_path):
        print(f"Fichier manquant : {file_path}")
        return

    # crEer ou rEcupErer la collection cible
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

    print(f"Import FBX : {file_path} - {parent_grp_name}")


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



def export_hierarchy_by_name(empty_names, filepath):
    """
    Exporte plusieurs empties et toute leur hierarchie en un FBX propre pour Maya.
    - Supprime les shaders/materials de l export
    - Ajuste l echelle (metres centimetres)
    """
    if not empty_names:
        print("[ERREUR] Aucun nom d'empty fourni.")
        return

    filepath = Path(filepath)
    filepath.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.object.select_all(action='DESELECT')

    def select_hierarchy(obj):
        """Selectionne recursivement un objet et ses enfants."""
        obj.select_set(True)
        for child in obj.children:
            select_hierarchy(child)

    found = False
    for name in empty_names:
        obj = bpy.data.objects.get(name)
        if obj is None:
            print(f"[AVERTISSEMENT] Aucun objet trouve : '{name}'")
            continue
        if obj.type != 'EMPTY':
            print(f"[AVERTISSEMENT] L'objet '{name}' n'est pas un Empty (type: {obj.type})")
            continue

        select_hierarchy(obj)
        if not found:
            bpy.context.view_layer.objects.active = obj
            found = True

    if not found:
        print("[ERREUR] Aucun empty valide trouve. Export annule.")
        return

    # Supprimer  les materiaux
    stored_materials = {}
    for obj in bpy.context.selected_objects:
        if hasattr(obj.data, "materials"):
            stored_materials[obj.name] = list(obj.data.materials)
            obj.data.materials.clear()

    # Export FBX 
    bpy.ops.export_scene.fbx(
        filepath=str(filepath),
        use_selection=True,
        object_types={'EMPTY', 'MESH', 'ARMATURE'},
        use_custom_props=False,
        bake_space_transform=False,
        apply_unit_scale=True,
        apply_scale_options='FBX_SCALE_UNITS',
        global_scale=1,
        add_leaf_bones=False,
        use_armature_deform_only=True,
        embed_textures=False,
        path_mode='AUTO',
    )


    print(f"[OK] Export FBX termine pour Maya : {filepath}")


def reset_scene(path):
    """
    Reset la scene avec le template Blender
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
    coll_to_empty = {}
    # # petit fix pour si la Col a deja _GRP
    # if (coll.name).endswith("_GRP"):
    #     coll.name = coll.name.replace("_GRP", "")
    #     # rename a la porc de la coll
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