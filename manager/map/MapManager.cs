using Godot;
using MyGame.Map;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public partial class MapManager : Node
    {
        private BasicMap _currentMap;
        private readonly Dictionary<string, PackedScene> _loadedMaps = new();

        [Signal]
        public delegate void MapTransitionCompleteEventHandler();

        public Vector2 GetLandmarkPosition(string landmarkName)
        {
            return _currentMap.GetLandmarkPosition(landmarkName);
        }

        public Rect2I GetUsedRect()
        {
            return _currentMap.GetUsedRect();
        }

        public void LoadMap(string mapName)
        {
            if (!_loadedMaps.TryGetValue(mapName, out var map))
            {
                map = ResourceManager.Instance.GetResource(mapName);
                _loadedMaps[mapName] = map;
            }

            if (_currentMap != null)
            {
                _currentMap.QueueFree();
                _currentMap = null;
            }

            _currentMap = map.Instantiate<BasicMap>();
            AddChild(_currentMap);

            EmitSignal(SignalName.MapTransitionComplete);
            GD.Print($"Map loaded successfully: {mapName}");
        }

        public void AfterTransitionComplete()
        {
            GD.Print($"Map transit to {_currentMap.GetMapName()} complete");
        }
    }
}
