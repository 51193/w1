using Godot;
using MyGame.Map;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class MapManager : Node
	{
		private Node _currentMap;
		private readonly Dictionary<string, PackedScene> _loadedMaps = new();

		[Signal]
		public delegate void MapTransitionCompleteEventHandler();

		public Vector2 GetLandmarkPosition(string landmarkName)
		{
			if (_currentMap is BaseMap map)
			{
				return map.GetLandmarkPosition(landmarkName);
			}
			else
			{
				GD.PrintErr("Invalid current map when getting landmark position in MapManager");
				return new Vector2(0, 0);
			}
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

			_currentMap = map.Instantiate();
			AddChild(_currentMap);

			EmitSignal(SignalName.MapTransitionComplete);
			GD.Print($"Map loaded successfully: {mapName}");
		}

		public void AfterTransitionComplete()
		{
			GD.Print($"Map transit to {((BaseMap)_currentMap).GetMapName()} complete");
		}
	}
}
