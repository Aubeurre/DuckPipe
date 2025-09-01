using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckPipe.Core.Manipulator;
using DuckPipe.Core.Services;

namespace DuckPipe.Core.Manager
{
    public class LockNodeFileManager
    {
        public static void TryLockFile(string nodePath, AssetManagerForm form)
        {
            string workFolderPath = Path.GetDirectoryName(nodePath);
            string fileName = Path.GetFileNameWithoutExtension(nodePath);
            string FileExt = Path.GetExtension(nodePath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            string lockedByUser = "";

            if (File.Exists(lockFile))
            {
                lockedByUser = File.ReadAllText(lockFile);
                return;
            }

            lockedByUser = ProductionService.GetUserName();
            File.WriteAllText(lockFile, lockedByUser);

            string[] nodeParts = nodePath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            form.RefreshTab(nodeParts[0]);
            NodeManip.CopyNodeToTemp(nodePath);
            return;
        }

        public static void UnlockFile(string nodePath, AssetManagerForm form)
        {
            string workFolderPath = Path.GetDirectoryName(nodePath);
            string fileName = Path.GetFileNameWithoutExtension(nodePath);
            string FileExt = Path.GetExtension(nodePath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            if (File.Exists(lockFile))
                File.Delete(lockFile);
            NodeManip.DeleteTemp(nodePath);

            string[] nodeParts = nodePath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            form.RefreshTab(nodeParts[0]);
        }
        public static string GetuserLocked(string nodePath)
        {
            string workFolderPath = Path.GetDirectoryName(nodePath);
            string fileName = Path.GetFileNameWithoutExtension(nodePath);
            string FileExt = Path.GetExtension(nodePath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            if (!File.Exists(lockFile))
            {
                return "";
            }
            else
            {
                string userLocked = File.ReadAllText(lockFile);
                return userLocked;
            }
        }

        public static bool IsLockedByUser(string nodePath)
        {
            string userLock = GetuserLocked(nodePath);
            if (userLock == ProductionService.GetUserName())
            {
                return true;
            }
            else { return false; }
        }

    }
}
