# -*- coding: utf-8 -*-
"""
Script pour maya
decrit la scene d'assemble en JSON
position des ctl de local_ctl, value de l'attr custom if animable or not (for futur, LOD...))

"""

import json
import os
import sys
import maya.cmds as cmds
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs


# ----------------------------
# Four tout 
# ----------------------------

def get_all_props_in_scene():
    props_info = {}
    global_ctls = cmds.ls("global_ctl", l=1) or []
    for ctl in global_ctls:
        print(ctl)
        name = cmds.getAttr(f"{ctl}.asset_name")
        pos = cmds.xform(ctl.replace('global_ctl','local_ctl'), q=True, ws=True, t=True)
        rot = cmds.xform(ctl.replace('global_ctl','local_ctl'), q=True, ws=True, ro=True)
        scale = cmds.xform(ctl.replace('global_ctl','local_ctl'), q=True, ws=True, scale=True)
        is_assembly_animable = cmds.getAttr(f"{ctl}.is_assembly_animable")
        props_info[name] = {
            "position": pos,
            "rotation": rot,
            "scale": scale,
            "is_assembly_animable": is_assembly_animable
        }
    return props_info

# ----------------------------
# Main exporter
# ----------------------------
def export(out_json_path):

    props_info = get_all_props_in_scene()
    GlobalProcs.write_json(props_info, out_json_path)
    print(f"[export_assemble] Exported {len(props_info)} props.")