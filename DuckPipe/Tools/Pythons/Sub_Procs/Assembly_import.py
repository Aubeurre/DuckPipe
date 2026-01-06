# -*- coding: utf-8 -*-
import maya.cmds as cmds
import os
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs


# ----------------------------
# Utils
# ----------------------------
def apply_transform(node, pos, rot, scale):
    print(node, pos, rot, scale)
    cmds.xform(node, ws=True, t=pos)
    cmds.xform(node, ws=True, ro=rot)
    cmds.xform(node, ws=True, s=scale)




# ----------------------------
# Main
# ----------------------------

def import_assembly_for_anim(assembly_json, prod_base_path, cach_only=True):
    data = GlobalProcs.read_json(assembly_json)

    imported = []

    for asset_name, info in data.items():      
        rig_path = f"{prod_base_path}/assets/Props/{asset_name}/dlv/{asset_name}_rig_OK.ma"
        model_path = f"{prod_base_path}/assets/Props/{asset_name}/dlv/{asset_name}_model.fbx"  
        placer = None

        if cach_only:  
            if not os.path.exists(model_path):
                print(f"[import_assembly] Model FBX not found for {asset_name} at {model_path}")
                continue
            else:
                placer , gpu_cache = MayaProcs.fbx_to_gpu_cache(model_path)
        else:
            if not info.get("is_assembly_animable"):  
                if not os.path.exists(model_path):
                    print(f"[import_assembly] Model FBX not found for {asset_name} at {model_path}")
                    continue
                else:
                    placer , gpu_cache = MayaProcs.fbx_to_gpu_cache(model_path)
            else:
                if not os.path.exists(rig_path):
                    print(f"[import_assembly] Model FBX not found for {asset_name} at {rig_path}")
                    continue
                else:
                    placer = MayaProcs.reference_animable_rig(rig_path, asset_name)
        if not placer:
            print(f"[import_assembly] Failed to import {asset_name}")
            continue
        else:
            apply_transform(
                placer,
                info["position"],
                info["rotation"],
                info["scale"]
            )

            imported.append(asset_name)

    print(f"[import_assembly] Imported {len(imported)} animable rigs")
    return imported

# Example usage:
# import_assembly_for_anim('I:/PROD/SPARK/Assets/Environments/KoreanTemple/dlv/assembly.json', "I:/PROD/SPARK")