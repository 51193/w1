using MyGame.Manager;
using MyGame.Util.MyGame.Util;
using System.IO;
using System.Text.Json;

namespace MyGame.Util
{
    public static class JsonUtil
    {
        public static string SerializeSaveData(SaveData saveData)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new Vector2Converter(),
                    new VariableLengthParameterConverter(),
                    new ISaveComponentConverter()
                }
            };
            return JsonSerializer.Serialize(saveData, options);
        }

        public static SaveData DeserializeSaveData(string json)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new Vector2Converter(),
                    new VariableLengthParameterConverter(),
                    new ISaveComponentConverter()
                }
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
