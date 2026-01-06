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
    
    prodName = os.path.basename(prod_path)

    if f"/{prodName}/" in file_path:
        local_path = file_path.split(f"/{prodName}/")[0]
        local_path = os.path.join(local_path, prodName).replace("\\", "/")
        return local_path
    else:
        return ""

def remove_envvar_from_path(path):
    """
    Supprime la variable d'environnement du type ${DUCKPIPE_ROOT} dans un chemin
    """
    if '${DUCKPIPE_ROOT}' in path:
        env_var = "${DUCKPIPE_ROOT}"
        env_value = os.environ.get("DUCKPIPE_ROOT", "")
        path = path.replace(env_var, env_value)
        #TODO la variable d env ne porte pas le meme nom