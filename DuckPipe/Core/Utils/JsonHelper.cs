using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public static void Save<T>(string filePath, T data, bool indented = false)
        {
            var options = new JsonSerializerOptions { WriteIndented = indented };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }

        public static T Load<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
