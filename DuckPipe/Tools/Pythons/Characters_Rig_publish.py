import os
import sys
import maya.cmds as cmds

RIGSUFFIX = "_rig_OK"
FACIALSUFFIX = "_facial_OK"
ASSEMBLESUFFIX = "_assemble_OK"


def publish():
    print("Pre-publish")
    

def createAssemble(file_name, file_path):
    newFileName = file_name.replace(RIGSUFFIX, ASSEMBLESUFFIX)
    newFilePath = file_path.replace(file_name, newFileName)
    print(f"Creation assemble: {newFilePath}")
    cmds.file(rename=newFilePath)
    cmds.file(save=True, type="mayaAscii")


def postpublish():
    print("Post-publish")


def addFacialToAssemble(published_file, facial_file_path):
    if os.path.exists(facial_file_path):
        print(f"Ajout du facial: {facial_file_path}")
        cmds.file(facial_file_path, i=True, type="mayaAscii",
                  ignoreVersion=True, ra=True,
                  mergeNamespacesOnClash=False,
                  namespace=":", options="v=0")
        # TODO: connections
        cmds.file(save=True, type="mayaAscii")
    else:
        print("Pas de facial trouve.")


def main(published_file):
    file_name = os.path.basename(published_file)
    facial_file_name = file_name.replace(RIGSUFFIX, FACIALSUFFIX)
    facial_file_path = published_file.replace(file_name, facial_file_name)

    print(f"{published_file}")

    cmds.file(published_file, open=True, force=True)

    if f"{RIGSUFFIX}.ma" in file_name:
        publish()
        createAssemble(file_name, published_file)
        addFacialToAssemble(published_file, facial_file_path)
        postpublish()
