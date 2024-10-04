using Godot;
using MyGame.Map;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class MapManager : Node
	{
		private Node _currentMap;
		private readonly Dictionary<string, PackedScene> _loadedScenes = new();

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

		public string GetAnimationPlayedAfterSpawn(string landmarkName)
		{
			if (_currentMap is BaseMap map)
			{
				return map.GetAnimationPlayedAfterSpawn(landmarkName);
			}
			else
			{
				GD.PrintErr("Invalid current map when getting animation for landmark in MapManager");
				return null;
			}
		}

		public void LoadMap(string mapName)
		{
			if (!_loadedScenes.TryGetValue(mapName, out var map))
			{
				map = GlobalObjectManager.GetResource(mapName);
				_loadedScenes[mapName] = map;
			}

			if (_currentMap != null)
			{
				_currentMap.QueueFree();
				_currentMap = null;
			}

			_currentMap = map.Instantiate();
			AddChild(_currentMap);

			EmitSignal(SignalName.MapTransitionComplete);
			GD.Print($"Map loaded successfully: {((BaseMap)_currentMap).GetMapName()}");
		}

		public void AfterTransitionComplete()
		{
			GD.Print($"Map transit to {((BaseMap)_currentMap).GetMapName()} complete");
		}
	}
}
