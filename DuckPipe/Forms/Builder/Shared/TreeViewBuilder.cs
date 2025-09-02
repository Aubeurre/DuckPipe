using System.Windows.Forms;
using DuckPipe.Core.Services;

namespace DuckPipe.Forms.Builder.Shared
{
    public static class TreeViewBuilder
    {

        public static string GetPathFromNode(TreeNode node)
        {
            string rootPath = ProductionService.GetProductionRootPath();
            return TreeViewBuilder.GetFullPathFromNode(node, rootPath);

        }
        public static string GetFullPathFromNode(TreeNode node, string rootPath)
        {
            List<string> parts = new();
            TreeNode current = node;
            while (current != null)
            {
                parts.Insert(0, current.Text);
                current = current.Parent;
            }
            return Path.Combine(rootPath, Path.Combine(parts.ToArray()));
        }

        public static TreeNode GetFolderHierarchy(string rootPath, string prodName)
        {
            string prodPath = Path.Combine(rootPath, prodName);
            if (!Directory.Exists(prodPath)) return null;

            TreeNode rootNode = new TreeNode(prodName) { Tag = prodPath };

            var folderDepthLimits = new Dictionary<string, int>
            {
                { "Assets", 3 },
                { "Shots", 5 },
                { "Renders", 1 },
                { "IO", 0 },
                { "Dev", 0 },
                { "RnD", 0 },
                { "Preprod", 3 },
            };

            foreach (string dir in Directory.GetDirectories(prodPath))
            {
                string folderName = Path.GetFileName(dir);
                TreeNode node = new TreeNode(folderName) { Tag = dir };
                rootNode.Nodes.Add(node);

                int maxDepth = folderDepthLimits.TryGetValue(folderName, out int depth) ? depth : 1;
                AddDirectoriesToTreeWithDepth(dir, node, 1, maxDepth);
            }

            return rootNode;
        }

        private static void AddDirectoriesToTreeWithDepth(string folderPath, TreeNode parentNode, int currentDepth, int maxDepth)
        {
            if (currentDepth >= maxDepth) return;

            foreach (string dir in Directory.GetDirectories(folderPath))
            {
                if (!dir.Contains("Work"))
                {
                    TreeNode node = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    parentNode.Nodes.Add(node);

                    AddDirectoriesToTreeWithDepth(dir, node, currentDepth + 1, maxDepth);
                }
            }
        }
    }
}
