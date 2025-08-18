using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Models
{
    public class TaskData
    {
        public string Department { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
    }
}
