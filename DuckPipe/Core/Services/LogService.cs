using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Services
{
    public class LogService
    {
        public static void EchoLog(string message)
        {
            // Implémentation du logging
            Console.WriteLine($"[ECHO] {DateTime.Now}: {message}");
            // MessageBox.Show(message);
        }
        public static void EchoErrorLog(string message)
        {
            // Implémentation du logging
            Console.WriteLine($"[ECHO] {DateTime.Now}: {message}");
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void EchoInfoLog(string message)
        {
            // Implémentation du logging
            Console.WriteLine($"[ECHO] {DateTime.Now}: {message}");
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void EchoSuccessLog(string message)
        {
            // Implémentation du logging
            Console.WriteLine($"[ECHO] {DateTime.Now}: {message}");
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void writeLog(string message)
        {
            // Implémentation du logging
            Console.WriteLine($"[ECHO] {DateTime.Now}: {message}");
        }

    }
}
