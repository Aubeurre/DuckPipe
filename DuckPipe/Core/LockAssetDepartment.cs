using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core
{
    public class LockAssetDepartment
    {
        public static void TryLockFile(string assetPath, AssetManagerForm form)
        {
            MessageBox.Show(assetPath);
            string workFolderPath = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string FileExt = Path.GetExtension(assetPath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            string lockedByUser = "";

            if (File.Exists(lockFile))
            {
                lockedByUser = File.ReadAllText(lockFile);
                return;
            }

            lockedByUser = Environment.UserName;
            File.WriteAllText(lockFile, lockedByUser);

            string[] assetParts = assetPath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            form.RefreshRightPanel(assetParts[0]);
            AssetManip.CopyAssetToTemp(assetPath);
            return;
        }

        public static void UnlockFile(string assetPath, AssetManagerForm form)
        {
            string workFolderPath = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string FileExt = Path.GetExtension(assetPath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            if (File.Exists(lockFile))
                File.Delete(lockFile);
            AssetManip.DeleteTemp(assetPath);

            string[] assetParts = assetPath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            form.RefreshRightPanel(assetParts[0]);
        }
        public static string GetuserLocked(string assetPath)
        {
            string workFolderPath = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string FileExt = Path.GetExtension(assetPath);
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

        public static bool IsLockedByUser(string assetPath)
        {
            string userLock = GetuserLocked(assetPath);
            if (userLock == Environment.UserName)
            {
                return true;
            }
            else { return false; }
        }

    }
}
