using Godot;
using MyGame.Util;
using System;
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

    //TODO: This manager's position in scene tree is not reasonable, but just work. Might need modification in future.
    public partial class SaveManager : Node
    {
        [Export]
        private string _saveFilePath;

        public List<SaveConfig> SaveConfigs;

        private List<SaveConfig> LoadConfigFromFile()
        {
            string configFilePath = _saveFilePath + "/config.json";
            FileUtil.EnsureDirectoryExists(configFilePath);
            string jsonText = File.ReadAllText(configFilePath);
            return JsonSerializer.Deserialize<List<SaveConfig>>(jsonText);
        }

        public void SaveConfigToFile()
        {
            var jsonText = JsonSerializer.Serialize(SaveConfigs, new JsonSerializerOptions { WriteIndented = true });
            string configFilePath = _saveFilePath + "/config.json";
            FileUtil.EnsureDirectoryExists(configFilePath);
            File.WriteAllText(configFilePath, jsonText);
        }

        public void Save(SaveConfig saveConfig)
        {
            saveConfig.IsNew = false;
            saveConfig.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MapTransition.Instance.ToSaveData(_saveFilePath + saveConfig.FileName + "data.json");
            SaveConfigToFile();
        }

        public void Load(SaveConfig saveConfig)
        {
            string filePrefix;
            if (saveConfig.IsNew)
            {
                filePrefix = "new/";
            }
            else
            {
                filePrefix = saveConfig.FileName;
            }
            MapTransition.Instance.FromSaveData(_saveFilePath + filePrefix + "data.json");
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
            SaveConfigs = LoadConfigFromFile();
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
