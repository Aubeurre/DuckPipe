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


# ----------------------------
# Four tout 
# ----------------------------
def write_json(data, out_path):
    os.makedirs(os.path.dirname(out_path), exist_ok=True)
    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    print(f"[export_assemble] Wrote JSON -> {out_path}")


def get_all_props_in_scene():
    props_info = {}
    print(cmds.ls(dag=1) )
    global_ctls = cmds.ls("global_ctl", l=1) or []
    for ctl in global_ctls:
        print(ctl)
        name = cmds.getAttr(f"{ctl}.asset_name")
        pos = cmds.xform(ctl, q=True, ws=True, t=True)
        is_assembly_animable = cmds.getAttr(f"{ctl}.is_assembly_animable")
        props_info[name] = {
            "position": pos,
            "is_assembly_animable": is_assembly_animable
        }
    return props_info

# ----------------------------
# Main exporter
# ----------------------------
def export(out_json_path):

    props_info = get_all_props_in_scene()
    write_json(props_info, out_json_path)
    print(f"[export_assemble] Exported {len(props_info)} props.")