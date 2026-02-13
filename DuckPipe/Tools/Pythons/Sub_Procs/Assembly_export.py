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
def find_global_ctl(node):
    parent = cmds.listRelatives(node, p=True, f=True)
    while parent:
        parent = parent[0]
        short = parent.split("|")[-1]

        if short.endswith("global_ctl"):
            return parent

        parent = cmds.listRelatives(parent, p=True, f=True)

    return None


def get_all_ctl_in_scene():
    all_ctl_info = {}
    for item in cmds.ls("*_ctl", l=1) or []:
        # on cree un dico des infos de chaque ctl si pas null
        pos = cmds.xform(item, q=True, ws=True, t=True)
        rot = cmds.xform(item, q=True, ws=True, ro=True)
        scale = cmds.xform(item, q=True, ws=True, scale=True)
        if not pos == [0,0,0] and not rot == [0,0,0] and not scale == [1,1,1]:
            all_ctl_info[item] = {
                "position": pos,
                "rotation": rot,
                "scale": scale
            }

    return all_ctl_info


def get_all_props_in_scene():
    props_info = {}
    all_local_ctls = cmds.ls("local_ctl", l=1) or []

    for local_ctl in all_local_ctls:
        global_ctl = find_global_ctl(local_ctl)
        name = cmds.getAttr(f"{global_ctl}.asset_name")
        is_assembly_animable = cmds.getAttr(f"{global_ctl}.is_assembly_animable")
        all_ctl_position = get_all_ctl_in_scene()

        props_info.setdefault(name, []).append({
            "is_assembly_animable": is_assembly_animable,
            "ctl_info": all_ctl_position
        })

    return props_info


# ----------------------------
# Main exporter
# ----------------------------
def export(out_json_path):

    props_info = get_all_props_in_scene()
    GlobalProcs.write_json(props_info, out_json_path)
    print(f"[export_assemble] Exported {len(props_info)} props.")