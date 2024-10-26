using Godot;
using System.IO;
using System.Text.Json;

namespace MyGame.Manager
{
    public partial class SaveManager : Node
    {
        public static string SerializeSaveData(SaveData saveData)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Serialize(saveData, options);
        }

        public static SaveData DeserializeSaveData(string json)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<SaveData>(json, options);
        }

        public static void WriteToFile(string filePath, string data)
        {
            File.WriteAllText(filePath, data);
        }

        public static string ReadFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
