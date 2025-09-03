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

            // Si on est en RIG, on ajoute le publish du layout
            if (deptUpper == "RIG")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_OK{ProductionService.GetFileMainExt(prodPath, "MODELING")}"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_cfx_OK{ProductionService.GetFileMainExt(prodPath, "CFX")}"));
                if (nodeType == "Character")
                {
                    refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                        $"{nodeName.ToLower()}_groom_OK{ProductionService.GetFileMainExt(prodPath, "GROOM")}"));
                }
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "CFX")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_OK{ProductionService.GetFileMainExt(prodPath, "MODELING")}"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "SURF")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_cfx_OK{ProductionService.GetFileMainExt(prodPath, "CFX")}"));
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_OK{ProductionService.GetFileMainExt(prodPath, "MODELING")}"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "GROOM")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_OK{ProductionService.GetFileMainExt(prodPath, "MODELING")}"));
            }
            // Si on est en CFX, on ajoute le publish du layout
            if (deptUpper == "FACIAL")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(publishPath),
                    $"{nodeName.ToLower()}_model_OK{ProductionService.GetFileMainExt(prodPath, "MODELING")}"));
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
            // Si on est en LIGHTING, on ajoute le publish du light de la sequence
            if (deptUpper == "LIGHTING")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(seqDlvPath),
                    $"{nodeName.ToLower()}_lighting_OK{ProductionService.GetFileMainExt(prodPath, "LIGHTING")}"));
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
            // Si on est en LIGHTING, on ajoute le light studio
            if (deptUpper == "LIGHTING")
            {
                refNodes.Add(Path.Combine(NodeManip.SetEnvVariables(prodPath), "Shots", "Templates",
                    $"studioLight{ProductionService.GetFileMainExt(prodPath, "LIGHTING")}"));
            }
            return refNodes;
        }

    }
}
