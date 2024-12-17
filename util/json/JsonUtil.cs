using MyGame.Manager;
using MyGame.Util.MyGame.Util;
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
    }
}
