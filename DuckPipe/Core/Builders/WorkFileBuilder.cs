using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckPipe.Core.Manipulator;
using DuckPipe.Core.Services;

namespace DuckPipe.Core.Builders
{
    public static class WorkFileBuilder
    {
        public static List<string> CreateBasicReferencesStructure(string nodeName, string nodeType, string deptUpper, string publishPath, string prodPath)
        {
            // Liste de refNodes par défaut
            var refNodes = new List<string>();

            if (deptUpper == "RIG")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model.fbx"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_helpers.fbx"));
                if (nodeType == "Character")
                {
                    refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                        $"{nodeName.ToLower()}_cfx_prez.fbx"));
                    refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                        $"{nodeName.ToLower()}_cfx.fbx"));
                    refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                        $"{nodeName.ToLower()}_groom.fbx"));
                }
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "CFX")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model.fbx"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_cfx_prez.fbx"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "SURF")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model.fbx"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_cfx.fbx"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "GROOM")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model.fbx"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_cfx.fbx"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "FACIAL")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model.fbx"));
            }
            return refNodes;
        }
        public static List<string> CreateShotBasicReferencesStructure(string nodeName, string deptUpper, string publishPath, string prodPath, string seqDlvPath)
        {
            // Liste de refNodes par défaut
            var refNodes = new List<string>();

            // Si on est en LAYOUT, on ajoute la caméra
            if (deptUpper == "LAYOUT")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(seqDlvPath),
                    $"{nodeName.ToLower()}_layout_OK{ProductionService.GetFileMainExt(prodPath, "LAYOUT")}"));
            }
            // Si on est en ANIM, on ajoute le publish du layout
            if (deptUpper == "ANIM")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_layout_OK{ProductionService.GetFileMainExt(prodPath, "LAYOUT")}"));
            }
            // Si on est en LIGHT, on ajoute le publish du light de la sequence
            if (deptUpper == "LIGHT")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(seqDlvPath),
                    $"{nodeName.ToLower()}_light_OK{ProductionService.GetFileMainExt(prodPath, "LIGHT")}"));
            }
            return refNodes;
        }
        public static List<string> CreateSequenceBasicReferencesStructure(string nodeName, string deptUpper, string publishPath, string prodPath)
        {
            // Liste de refNodes par défaut
            var refNodes = new List<string>();

            // Si on est en LAYOUT, on ajoute la caméra
            if (deptUpper == "LAYOUT")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(prodPath), "Shots", "Templates",
                    $"studioCamera{ProductionService.GetFileMainExt(prodPath, "LAYOUT")}"));
            }
            // Si on est en LIGHT, on ajoute le light studio
            if (deptUpper == "LIGHT")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(prodPath), "Shots", "Templates",
                    $"studioLight{ProductionService.GetFileMainExt(prodPath, "LIGHT")}"));
            }
            return refNodes;
        }

    }
}
