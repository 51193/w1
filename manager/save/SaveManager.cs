using Godot;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MyGame.Manager
{
    public class SaveConfig
    {
        public string Title { get; set; }
        public bool IsNew { get; set; }
        public string Timestamp { get; set; }
        public string FileName { get; set; }
    }

    public partial class SaveManager : Node
    {
        [Export]
        private string _saveFilePath;

        public List<SaveConfig> SaveConfigs;

        private List<SaveConfig> LoadFromFile()
        {
            string jsonText = File.ReadAllText(_saveFilePath + "/config.json");
            return JsonSerializer.Deserialize<List<SaveConfig>>(jsonText);
        }

        public void SaveToFile()
        {
            var jsonText = JsonSerializer.Serialize(SaveConfigs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_saveFilePath + "/config.json", jsonText);
        }

        private static SaveManager _instance;
        public static SaveManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GD.PrintErr("ResourceManager is not available");
                }
                return _instance;
            }
        }

        public override void _EnterTree()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                GD.PrintErr("Duplicate ResourceManager entered the tree, this is not allowed");
            }
            SaveConfigs = LoadFromFile();
        }

        public override void _ExitTree()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
