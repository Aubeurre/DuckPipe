import os
import sys
    
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

