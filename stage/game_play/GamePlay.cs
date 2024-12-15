using Godot;
using MyGame.Manager;
using System;

namespace MyGame.Stage
{
    public partial class GamePlay : BasicStage
    {
        [Export]
        private MapTransition _mapTransition;

        private SaveConfig _saveConfig;

        public void InitMap(SaveConfig config)
        {
            _saveConfig = config;
            string filePrefix;
            if (_saveConfig.IsNew)
            {
                filePrefix = "new/";
            }
            else
            {
                filePrefix = _saveConfig.FileName;
            }
            _mapTransition.FromSaveData("save/" + filePrefix + "data.json");
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustReleased("save"))
            {
                _saveConfig.IsNew = false;
                _saveConfig.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                MapTransition.Instance.ToSaveData("save/" + _saveConfig.FileName + "data.json");
                SaveManager.Instance.SaveToFile();
            }

            if (Input.IsActionJustReleased("load"))
            {
                string filePrefix;
                if (_saveConfig.IsNew)
                {
                    filePrefix = "new/";
                }
                else
                {
                    filePrefix = _saveConfig.FileName;
                }
                _mapTransition.FromSaveData("save/" + filePrefix + "data.json");
            }
        }
    }
}
