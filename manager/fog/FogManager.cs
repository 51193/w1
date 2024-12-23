using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyGame.Manager
{
    public partial class FogManager : Node
    {
        [Export]
        private PackedScene _fogTemplate;

        private readonly Dictionary<string, Fog> _globalFogDictionary = new();
        private Fog _currentFog;

        private void SetCurrentFog(Fog currentFog, Vector2I position)
        {
            if (_currentFog != null)
            {
                RemoveChild(_currentFog);
            }
            _currentFog = currentFog;
            _currentFog.ApplyFogImage();
            _currentFog.ApplyFogTexture(position);
            AddChild(_currentFog);
            GD.Print("Fog transition complete");
        }

        public void TryLoadFog(string mapName, Rect2I usedRect)
        {
            if (!_globalFogDictionary.ContainsKey(mapName))
            {
                Fog fog = _fogTemplate.Instantiate<Fog>();
                fog.CreateFogImage(usedRect.Size);
                _globalFogDictionary[mapName] = fog;
                GD.Print("Create a new fog image");
            }
            SetCurrentFog(_globalFogDictionary[mapName], usedRect.Position);
        }

        public void EraseCurrentFog(Image bitMap, Vector2I bitmapPosition)
        {
            if(_currentFog == null)
            {
                GD.PrintErr("Current map is invalid, cannot erase fog");
                return;
            }

            _currentFog.EraseFog(bitMap, bitmapPosition);
        }

        public void LoadAllFogImagesFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                GD.PrintErr($"Folder does not exist: {folderPath}");
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.png");
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                string key = Path.GetFileNameWithoutExtension(fileName);

                Fog fog = _fogTemplate.Instantiate<Fog>();
                fog.LoadFogImage(filePath);

                _globalFogDictionary[key] = fog;
            }

            GD.Print($"All fogs loaded from folder: {folderPath}");
        }

        public void SaveAllFogImagesToFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e)
                {
                    GD.PrintErr($"Failed to create folder: {folderPath}; Exception: {e.Message}");
                    return;
                }
            }

            foreach (var key in _globalFogDictionary.Keys)
            {
                string filePath = Path.Combine(folderPath, $"{key}.png");
                Fog fog = _globalFogDictionary[key];
                fog.SaveFogImage(filePath);
            }

            GD.Print($"All fogs saved to folder: {folderPath}");
        }

        private static FogManager _instance;
        public static FogManager Instance
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
