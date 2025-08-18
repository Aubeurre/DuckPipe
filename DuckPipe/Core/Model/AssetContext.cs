using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Model
{
    public class AssetContext
    {
        public string RootPath { get; set; }
        public string ProductionName { get; set; }
        public string ProductionPath { get; set; }
        public string AssetType { get; set; }
        public string AssetName { get; set; }
        public string? Department { get; set; }
        public string AssetRoot { get; set; }
        public string SequenceName { get; set; }
    }
}
