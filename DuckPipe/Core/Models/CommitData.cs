using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Model
{
    internal class CommitData
    {
        public string Version { get; set; }
        public string User { get; set; } // Auteur du commit
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string Status { get; set; } // Statut de la tâche à ce commit
        public string Department { get; set; } // Optionnel
    }
}
