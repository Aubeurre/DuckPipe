using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core
{
    internal class LockAssetDepartment
    {
        public static bool TryLockFile(string assetPath, out string lockedByUser)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string lockFile = Path.Combine(assetPath, $"{fileName}.lock");

            if (File.Exists(lockFile))
            {
                lockedByUser = File.ReadAllText(lockFile);
                return false;
            }

            lockedByUser = Environment.UserName;
            File.WriteAllText(lockFile, lockedByUser);
            return true;
        }

        public static void UnlockFile(string assetPath, string file)
        {
            string lockFile = Path.Combine(assetPath, $"{file}.lock");
            if (File.Exists(lockFile))
                File.Delete(lockFile);
        }
    }
}
