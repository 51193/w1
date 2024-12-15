using Godot;
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
                    new TypeConverter(),
                    new Vector2Converter(),
                    new VariableLengthParameterConverter(),
                    new ISaveComponentConverter(),
                    new TypeBasicDataDictionaryConverter()
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
                    new TypeConverter(),
                    new Vector2Converter(),
                    new VariableLengthParameterConverter(),
                    new ISaveComponentConverter(),
                    new TypeBasicDataDictionaryConverter()
                }
            };
            return JsonSerializer.Deserialize<SaveData>(json, options);
        }

        private static void EnsureDirectoryExists(string relativeFilePath)
        {
            string directoryPath = Path.GetDirectoryName(relativeFilePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                GD.Print($"Created directory: {directoryPath}");
            }
            else
            {
                GD.Print($"Directory already exists: {directoryPath}");
            }
        }

        public static void WriteToFile(string filePath, string data)
        {
            EnsureDirectoryExists(filePath);
            File.WriteAllText(filePath, data);
        }

        public static string ReadFromFile(string filePath)
        {
            EnsureDirectoryExists(filePath);
            return File.ReadAllText(filePath);
        }
    }
}
