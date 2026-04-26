import json
import os
import maya.cmds as cmds
from Soft_Procs import GlobalProcs

import json
import os
import copy


# --------------------------------------------------
# UPDATE SHOT JSON FROM LAYOUT JSON
# --------------------------------------------------
def update_shots_from_layout(out_json_path):

    if not os.path.exists(out_json_path):
        print(f"[ERROR] Layout JSON not found: {out_json_path}")
        return

    # -----------------------------
    # LOAD LAYOUT JSON
    # -----------------------------
    with open(out_json_path, "r") as f:
        layout_data = json.load(f)

    print(f"[INFO] Loaded layout JSON with {len(layout_data)} shots")

    # -----------------------------
    # PATHS
    # -----------------------------
    dlv_dir = os.path.dirname(out_json_path)
    seq_dir = os.path.dirname(dlv_dir)
    shots_root = os.path.join(seq_dir, "Shots")

    # -----------------------------
    # LOAD SEQUENCE NODE.JSON
    # -----------------------------
    seq_node_json = os.path.join(seq_dir, "node.json")

    if not os.path.exists(seq_node_json):
        print("[ERROR] Sequence node.json not found")
        return

    with open(seq_node_json, "r") as f:
        seq_data = json.load(f)

    seq_ref_assets = seq_data.get("nodeInfos", {}).get("refAssets", [])

    # -----------------------------
    # LOOP SHOTS
    # -----------------------------
    for shot_name, data in layout_data.items():

        in_frame = data.get("start_frame")
        out_frame = data.get("end_frame")

        shot_json_path = os.path.join(shots_root, shot_name, "node.json")

        if not os.path.exists(shot_json_path):
            print(f"[WARN] Missing node.json for {shot_name}")
            continue

        # load shot json
        with open(shot_json_path, "r") as f:
            shot_data = json.load(f)

        # -----------------------------
        # ENSURE nodeInfos
        # -----------------------------
        if "nodeInfos" not in shot_data:
            shot_data["nodeInfos"] = {}

        # -----------------------------
        # UPDATE FRAMES (STRING)
        # -----------------------------
        shot_data["nodeInfos"]["inFrame"] = str(int(in_frame)) if in_frame is not None else ""
        shot_data["nodeInfos"]["outFrame"] = str(int(out_frame)) if out_frame is not None else ""

        # -----------------------------
        # COPY refAssets (SAFE + SEQ ADD + ENV VAR)
        # -----------------------------
        SCRIPT_FILE = os.path.abspath(__file__)
        PROD_PATH = GlobalProcs.get_prodpath_from_pythonpath(SCRIPT_FILE)

        if "nodeInfos" not in shot_data:
            shot_data["nodeInfos"] = {}

        existing_refs = shot_data["nodeInfos"].get("refAssets", [])

        seq_refs = copy.deepcopy(seq_ref_assets)

        seq_refs = [            
            GlobalProcs.inject_envvar_in_path(r.replace("\\", "/"))
            for r in seq_refs
        ]
        print(PROD_PATH)
        seq_path = GlobalProcs.inject_envvar_in_path(
            dlv_dir.replace("\\", "/").replace(PROD_PATH, f"${{{'DUCKPIPE_ROOT'}}}")
        )
        seq_refs.append(seq_path)
        print(seq_path,seq_path,seq_path,seq_path,seq_path)
        print(seq_path,seq_path,seq_path,seq_path,seq_path)
        print(seq_path,seq_path,seq_path,seq_path,seq_path)
        print(seq_path,seq_path,seq_path,seq_path,seq_path)

        existing_refs = [
            GlobalProcs.inject_envvar_in_path(r.replace("\\", "/"))
            for r in existing_refs
        ]

        shot_data["nodeInfos"]["refAssets"] = list(
            dict.fromkeys(seq_refs + existing_refs)
        )

        # -----------------------------
        # SAVE
        # -----------------------------
        with open(shot_json_path, "w") as f:
            json.dump(shot_data, f, indent=2)

        print(f"[UPDATED] {shot_name} -> frames + refAssets synced")

# --------------------------------------------------
# SHOTS INFO
# --------------------------------------------------
def get_shots_info():
    layout_info = {}

    shots = cmds.ls(type='shot') or []
    print(f"[Layout] Found {len(shots)} shots")

    for shot in shots:
        shot_name = cmds.getAttr(f"{shot}.shotName")
        start = cmds.getAttr(f"{shot}.startFrame")
        end = cmds.getAttr(f"{shot}.endFrame")

        cam = cmds.listConnections(f"{shot}.currentCamera")
        if not cam:
            print(f"[WARN] No camera for {shot_name}")
            continue

        layout_info[shot_name] = {
            "start_frame": int(start),
            "end_frame": int(end),
            "camera": cam[0]
        }

    return layout_info


# --------------------------------------------------
# BAKE + EXPORT CAMERA
# --------------------------------------------------
def export_camera_animation(camera, start, end, out_path):

    if not cmds.objExists(camera):
        print(f"[ERROR] Camera not found: {camera}")
        return

    # duplicate clean camera
    dup = cmds.duplicate(camera, name=f"{camera}_EXPORT")[0]

    # unlock attrs (safe)
    for attr in ["tx","ty","tz","rx","ry","rz","sx","sy","sz"]:
        if cmds.objExists(f"{dup}.{attr}"):
            cmds.setAttr(f"{dup}.{attr}", lock=False)

    # bake animation
    cmds.bakeResults(
        dup,
        t=(start, end),
        simulation=True,
        sampleBy=1,
        preserveOutsideKeys=False,
        sparseAnimCurveBake=False
    )

    # export FBX
    cmds.select(dup, r=True)

    cmds.file(
        out_path,
        force=True,
        options="v=0;",
        type="FBX export",
        exportSelected=True
    )

    print(f"[EXPORT] {out_path}")

    cmds.delete(dup)


# --------------------------------------------------
# EXPORT ALL CAMS
# --------------------------------------------------
def export_all_cams(layout_info, out_json_path):

    base_dir = os.path.dirname(out_json_path)

    for shot_name, data in layout_info.items():

        start = data["start_frame"]
        end = data["end_frame"]
        cam = data["camera"]

        # main export
        fbx_name = f"{shot_name}_camera.fbx"
        fbx_path = os.path.join(base_dir, fbx_name)

        export_camera_animation(cam, start, end, fbx_path)

        # optional shot folder export
        shot_folder = os.path.join(
            os.path.dirname(base_dir),
            shot_name,
            "dlv"
        )

        if os.path.exists(shot_folder):
            fbx_path_2 = os.path.join(shot_folder, fbx_name)
            export_camera_animation(cam, start, end, fbx_path_2)


# --------------------------------------------------
# OVERRIDE EXPORT (single cam)
# --------------------------------------------------
def export_override(out_json_path):

    sel = cmds.ls(sl=True)
    if not sel:
        cmds.warning("Select a camera")
        return

    cam = sel[0]

    start = cmds.playbackOptions(q=True, min=True)
    end = cmds.playbackOptions(q=True, max=True)

    scene_name = os.path.splitext(os.path.basename(cmds.file(q=True, sn=True)))[0]

    out_path = os.path.join(
        os.path.dirname(out_json_path),
        f"{scene_name}_camera.fbx"
    )

    export_camera_animation(cam, start, end, out_path)


# --------------------------------------------------
# MAIN EXPORT
# --------------------------------------------------
def export(out_json_path):

    layout_info = get_shots_info()

    # write JSON
    GlobalProcs.write_json(layout_info, out_json_path)

    print(f"[JSON] Exported: {out_json_path}")

    # export cams
    export_all_cams(layout_info, out_json_path)

    return out_json_path