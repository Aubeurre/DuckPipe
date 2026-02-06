using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Services
{
    public class LogService
    {
        public static event Action<string> OnLog;

        public static void EchoLog(string message)
        {
            // Implémentation du logging
            // MessageBox.Show(message);
            OnLog?.Invoke(message);
        }
        public static void EchoErrorLog(string message)
        {
            // Implémentation du logging
        }
        public static void EchoInfoLog(string message)
        {
            // Implémentation du logging
            OnLog?.Invoke($"INFO: {message}");
        }
        public static void EchoSuccessLog(string message)
        {
            // Implémentation du logging
            OnLog?.Invoke($"SUCCESS: {message}");
        }

    }
}
