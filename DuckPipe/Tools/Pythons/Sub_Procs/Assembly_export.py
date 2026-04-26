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

def clean_ctl_name(node):
    short = node.split("|")[-1]  # enlève long path
    short = short.split(":")[-1]  # enlève namespace
    return short


def get_ctl_under_root(root):
    all_ctl_info = {}

    descendants = cmds.listRelatives(root, ad=True, f=True) or []

    for node in descendants:
        if not node.endswith("_ctl"):
            continue

        short = node.split("|")[-1].split(":")[-1]

        pos = cmds.xform(node, q=True, ws=True, t=True)
        rot = cmds.xform(node, q=True, ws=True, ro=True)
        scale = cmds.xform(node, q=True, ws=True, scale=True)

        all_ctl_info[short] = {
            "position": pos,
            "rotation": rot,
            "scale": scale
        }

    return all_ctl_info



def get_all_props_in_scene():
    props_info = {}
    all_local_ctls = cmds.ls("local_ctl", l=True) or []

    for local_ctl in all_local_ctls:

        global_ctl = find_global_ctl(local_ctl)

        name = cmds.getAttr(f"{global_ctl}.asset_name")
        is_assembly_animable = cmds.getAttr(f"{global_ctl}.is_assembly_animable")

        ctl_data = get_ctl_under_root(global_ctl)

        props_info.setdefault(name, []).append({
            "is_assembly_animable": is_assembly_animable,
            "ctl_info": ctl_data
        })

    return props_info



# ----------------------------
# Main exporter
# ----------------------------
def export(out_json_path):

    props_info = get_all_props_in_scene()
    GlobalProcs.write_json(props_info, out_json_path)
    print(f"[export_assemble] Exported {len(props_info)} props.")