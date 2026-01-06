"""    
Export layout data to a JSON file for MAYA

On cherche a extraire des infos:
 - le frame in et out de chaque shot (on regarde le sequencer de maya pour ca)
 - l'animation de la camera de la sequence (on exporte l'anim de la camera pour chaque shot)

"""

import json
import os
import sys
import maya.cmds as cmds
from Soft_Procs import MayaProcs
from Soft_Procs import GlobalProcs

def export_overide(out_json_path):
    # pour overide juste la cam directement depuis le shot et plus la sequence complete
    cam = get_camera()
    start_frame = cmds.playbackOptions(q=True, min=True)
    end_frame = cmds.playbackOptions(q=True, max=True)
    shot_name = os.path.splitext(os.path.basename(cmds.file(q=True, sn=True)))[0]
    export_camera_animation(cam, start_frame, end_frame, out_json_path.replace(".json", f"_{shot_name}_camera.fbx"))


def export_infos(out_json_path):
    layout_info = {}
    # on cherche les shots dans le sequencer
    shots = cmds.sequencer(q=True, shots=True) or []
    print(f"[export_layout] Found {len(shots)} shots in sequencer.")
    for shot in shots:
        shot_name = cmds.sequencer(shot, q=True, name=True)
        start_frame = cmds.sequencer(shot, q=True, startFrame=True)
        end_frame = cmds.sequencer(shot, q=True, endFrame=True)
        layout_info[shot_name] = {
            "start_frame": start_frame,
            "end_frame": end_frame
        }
        print(f"[export_layout] Shot: {shot_name}, Start: {start_frame}, End: {end_frame}")

    GlobalProcs.write_json(layout_info, out_json_path)
    return layout_info


def export_camera_animation(camera_name, start_frame, end_frame, out_fbx_path):
    """
    Export the animation of the given camera between start_frame and end_frame to an FBX file.
    """
    cmds.select(camera_name, r=True)
    cmds.cutKey(camera_name, time=(start_frame, end_frame))
    cmds.file(out_fbx_path, force=True, options="v=0;", type="FBX export", exportSelected=True)
    print(f"[export_camera_animation] Exported camera animation to {out_fbx_path}")


def get_camera():
    """
    on get la camera qui sera la seul hors celles de base de maya (persp, top, front...)
    on exclu donc les cameras par defaut (celle qu on veut est en reference)
    """
    all_cameras = cmds.ls(cameras=True)
    for cam in all_cameras:
        if cam not in ['frontShape', 'perspShape', 'sideShape', 'topShape']:
            print(f"[get_camera] Found camera: {cam}")
            return cam
    print("[get_camera] No custom camera found.")
    return None


def export(out_json_path):
    layout_info = export_infos(out_json_path)
    print(f"[Layout_export] Export completed: {out_json_path}")
    cam = get_camera()
    # on export une cam par shot
    for shot_name, info in layout_info.items():
        start_frame = info['start_frame']
        end_frame = info['end_frame']
        shot_fbx_path = out_json_path.replace(".json", f"_{shot_name}_camera.fbx")
        export_camera_animation(cam, start_frame, end_frame, shot_fbx_path)
        # on export aussi les cam dans les dossiers des shots si ils existent
        shot_folder = os.path.join(os.path.dirname(os.path.dirname(out_json_path)), shot_name, "dlv")
        if os.path.exists(shot_folder):
            shot_fbx_path_in_folder = os.path.join(shot_folder, f"{shot_name}_camera.fbx")
            export_camera_animation(cam, start_frame, end_frame, shot_fbx_path_in_folder)