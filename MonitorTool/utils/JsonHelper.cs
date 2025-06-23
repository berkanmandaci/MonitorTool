using System.IO;
using System.Text.Json;

namespace MonitorTool.utils
{
    public static class JsonHelper
    {
        public static void SaveToFile<T>(string path, T obj)
        {
            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static T LoadFromFile<T>(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
} 