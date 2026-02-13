# -*- coding: utf-8 -*-
import maya.cmds as cmds
import os
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs


# ----------------------------
# Main
# ----------------------------
def inject_assembly_into_ma(ma_path, assembly_data, prod_base_path):
    """
    import all rigs in assembly scene

    Args:
        ma_path (scene file path): scene file path
        assembly_data (json): json file with all informations
        prod_base_path (str): base path for production (local is better)
    """

    for asset_name, info in assembly_data.items():

        rig_path = f"{prod_base_path}/assets/Props/{asset_name}/dlv/{asset_name}_rig_OK.ma"

        if not os.path.exists(rig_path):
            print(f"[assembly] missing: {asset_name}")
            continue

        namespace = asset_name

        MayaProcs.inject_reference_into_ma(
            ma_path,
            rig_path,
            namespace
        )

    print("[assembly] injection complete")


def import_assembly_for_anim(EXECUTED_FILE, assembly_json, prod_base_path, cach_only=False):
    data = GlobalProcs.read_json(assembly_json)

    imported = []

    for asset_name, instances in data.items():

        rig_path = f"{prod_base_path}/assets/Props/{asset_name}/dlv/{asset_name}_rig_OK.ma"
        model_path = f"{prod_base_path}/assets/Props/{asset_name}/dlv/{asset_name}_model.fbx"

        for i, info in enumerate(instances):

            namespace = f"{asset_name}_{i+1:02d}"

            if cach_only:
                if not os.path.exists(model_path):
                    print(f"[import_assembly] Model FBX not found for {asset_name} at {model_path}")
                    continue
                else:
                    object = MayaProcs.fbx_to_gpu_cache(model_path)
                    place_gpucache(object, info["local_ctl"])

            else:
                if not info.get("is_assembly_animable"):
                    if not os.path.exists(model_path):
                        print(f"[import_assembly] Model FBX not found for {asset_name} at {model_path}")
                        continue
                    else:
                        print("fbx_to_gpu_cache")
                        object = MayaProcs.fbx_to_gpu_cache(model_path)
                        place_gpucache(object, info["local_ctl"])

                else:
                    if not os.path.exists(rig_path):
                        print(f"[import_assembly] Rig not found for {asset_name} at {rig_path}")
                        continue
                    else:
                        print("reference_scene")
                        MayaProcs.reference_scene(rig_path, namespace)
                        place_all_imported_ctl([namespace], data)

            imported.append(namespace)

    print(f"[import_assembly] Imported {len(imported)} instances")
    return imported


def place_gpucache(object, info):
    MayaProcs.apply_transform(
        object,
        info["position"],
        info["rotation"],
        info["scale"]
    )

    
def place_all_imported_ctl(imported_namespaces, assembly_data):
    for namespace in imported_namespaces:
        asset_name = namespace.rsplit("_", 1)[0]
        index = int(namespace.rsplit("_", 1)[1]) - 1

        info = assembly_data.get(asset_name, [])[index]

        if not info:
            print(f"[place_all_imported_ctl] No data found for {namespace}")
            continue
        # on run sur tous les ctl pas juste le local
        all_imported_ctl = cmds.ls(f"{namespace}:*_ctl", l=1) or []
        for ctl in all_imported_ctl:
            MayaProcs.apply_transform(
                ctl,
                info["position"],
                info["rotation"],
                info["scale"]
            )