using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DuckPipe.Core.Utils
{
    internal class JsonHelper
    {
        public static JsonNode ParseJson(string json)
        {
            string jsonString = File.ReadAllText(json);
            JsonNode jsonNode = JsonNode.Parse(jsonString);
            return jsonNode;
        }
    }
}
