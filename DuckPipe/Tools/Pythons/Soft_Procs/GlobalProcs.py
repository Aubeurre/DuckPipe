import os
import sys
import json
    

def read_json(json_path):
    """
    Lit un fichier JSON 
    """
    import json

    if not os.path.exists(json_path):
        print(f"[read_json] File not found: {json_path}")
        return {}

    with open(json_path, "r", encoding="utf-8") as f:
        data = json.load(f)

    return data


def write_json(data, out_path):
    os.makedirs(os.path.dirname(out_path), exist_ok=True)
    with open(out_path, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    print(f"[export_assemble] Wrote JSON -> {out_path}")



def get_asset_dependencies(asset_path):
    """
    Retourne la liste des dépendances d'un asset
    elles sont listées dans le fichier node.json dans le dossier de l'asset. sous refAssets
        {...
        "nodeInfos": {
            "refAssets": [
            "${DUCKPIPE_ROOT}/SPARK/Assets/Environments/KoreanTemple/dlv",
            "${DUCKPIPE_ROOT}/SPARK/Assets/Characters/Blue/dlv",
            "${DUCKPIPE_ROOT}/SPARK/Assets/Characters/Red/dlv"
            ]
            },...
        }   
    """
    asset_path = remove_envvar_from_path(asset_path)
    node_json_path = os.path.join(asset_path, "node.json")
    print(f"[get_asset_dependencies] Reading node.json: {node_json_path}")
    node_data = read_json(node_json_path)
    dependencies = []

    ref_assets = node_data.get("nodeInfos", {}).get("refAssets", [])
    for ref in ref_assets:
        ref = ref.replace("\\", "/")
        ref = remove_envvar_from_path(ref)
        parts = ref.split("/Assets/")
        if len(parts) < 2:
            continue
        asset_info = parts[1].split("/dlv")[0].split("/")
        if len(asset_info) < 2:
            continue
        asset_type = asset_info[0]
        asset_name = asset_info[1]
        dependencies.append({
            "type": asset_type,
            "name": asset_name,
            "path": ref
        })
    print(f"[get_asset_dependencies] Found {len(dependencies)} dependencies.")
    return dependencies


def get_prodpath_from_pythonpath(py_path):
    """
    Retourne le chemin prod a partir du chemin python
    """
    if not py_path:
        return ""
    py_path = py_path.replace("\\", "/")
    if "/Dev/" in py_path:
        prod_path = py_path.split("/Dev/")[0]
        return prod_path
    else:
        return ""


def get_local_path_from_filepath(file_path, prod_path):
    """
    Retourne le chemin local a partir du chemin complet
    """
    if not file_path:
        return ""
    print((file_path, prod_path))
    file_path = file_path.replace("\\", "/")
    prod_path = prod_path.replace("\\", "/")
    
    prodName = os.path.basename(prod_path)

    if f"/{prodName}/" in file_path:
        local_path = file_path.split(f"/{prodName}/")[0]
        local_path = os.path.join(local_path, prodName).replace("\\", "/")
        return local_path
    else:
        return ""


def resolve_envvar_in_path(path):
    """
    Remplace ${DUCKPIPE_ROOT} par sa valeur réelle
    """
    env_name = "DUCKPIPE_ROOT"
    token = f"${{{env_name}}}"

    if token in path:
        env_value = os.environ.get(env_name)
        if not env_value:
            raise RuntimeError(f"Environment variable {env_name} not found")
        path = path.replace(token, env_value)

    return path


def inject_envvar_in_path(path):
    """
    Remplace le root absolu par ${DUCKPIPE_ROOT}
    """
    env_name = "DUCKPIPE_ROOT"
    env_value = os.environ.get(env_name)

    if not env_value:
        raise RuntimeError(f"Environment variable {env_name} not found")

    normalized_path = os.path.normpath(path)
    normalized_env = os.path.normpath(env_value)

    if normalized_path.startswith(normalized_env):
        path = path.replace(normalized_env, f"${{{env_name}}}")

    return path


def getAllShots(path):
    """
    Get all shots from given seq
    get it by reading prod all nodes file.

    Args:
        path (string): path from sequence
    
    Return all_shots(list): a list of dict with all shots info
    """
    shots_dir = os.path.join(path, 'Shots')
    all_shots = []

    for item in os.listdir(shots_dir):
        full_shot_path = os.path.join(shots_dir, item, 'node.json')
        shot_node_json = read_json(full_shot_path)        
        nodeInfos = shot_node_json.get("nodeInfos", {})

        shot_info = {"name": item, 
                     'inframe': nodeInfos.get("inframe", 0), 
                     'outframe': nodeInfos.get("ounframe", 10)}
        
        print('found', shot_info)
        all_shots.append(shot_info)
    
    return all_shots

