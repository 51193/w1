using Godot;
using MyGame.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public class TransitionInfo
	{
		public string Departure { get; set; }
		public string Exit { get; set; }
		public string Destination { get; set; }
		public string EntryFrom { get; set; }
		public string EntryTo { get; set; }
	}

	public partial class MapTransition : Node
	{
		private MapManager _mapManager;
		private UnifiedEntityManager _unifiedEntityManager;

		private Dictionary<(string Departure, string Exit), TransitionInfo> _transitionsDict;

		private bool _mapManagerReady = false;
		private bool _entityManagerReady = false;

		[Signal]
		public delegate void InitMapEventHandler(string mapName);
		[Signal]
		public delegate void TransitMapEventHandler(string departureName, string exitName, Vector2 exitPosition, BaseDynamicEntity entity);

		private void LoadTransitionInfo(string path)
		{
			try
			{
				string jsonContent = File.ReadAllText(path);
				List<TransitionInfo> transitions = JsonSerializer.Deserialize<List<TransitionInfo>>(jsonContent);

				_transitionsDict = transitions.ToDictionary(
					t => (t.Departure, t.Exit),
					t => t
					);

				foreach (var transition in transitions)
				{
					GD.Print($"{transition.Departure}: {transition.Exit}----{transition.Destination}: {transition.EntryTo}");
				}
			}
			catch (Exception e)
			{
				GD.PrintErr($"Failed to load transition information path: {e.Message}");
				return;
			}
		}

		private void InitMapProcess(string mapName)
		{
			_mapManager.LoadMap(mapName);
			_unifiedEntityManager.InitiateEntities(mapName);
		}

		private void InvokeManagers(string destinationName, string fromLandmarkName, string toLandmarkName, BaseDynamicEntity entity)
		{
			_mapManager.LoadMap(destinationName);
			Vector2 fromLandmarkPosition = _mapManager.GetLandmarkPosition(fromLandmarkName);
			Vector2 toLandmarkPosition = _mapManager.GetLandmarkPosition(toLandmarkName);
			string animationToPlayForNewSpawnEntity = _mapManager.GetAnimationPlayedAfterSpawn(fromLandmarkName);
			_unifiedEntityManager.OnMapChanged(entity, destinationName, fromLandmarkPosition, toLandmarkPosition, animationToPlayForNewSpawnEntity);
		}

		private async void TransitionProcess(string departureName, string exitName, Vector2 exitPosition, BaseDynamicEntity entity)
		{
			entity.IsTransitable = false;
			entity.SetTookOverPosition((entity.Position - exitPosition).Length() * (float)1.5, exitPosition);
			entity.CollisionMask = 0;
			await Task.Delay(500);
			if (_transitionsDict.TryGetValue((departureName, exitName), out var transition))
			{
				CallDeferred(nameof(InvokeManagers), transition.Destination, transition.EntryFrom, transition.EntryTo, entity);
			}
			else
			{
				GD.PrintErr($"No transition found for: {departureName}-{exitName}");
			}
		}

		private void OnMapManagerComplete()
		{
			_mapManagerReady = true;
			CheckManagerState();
		}

		private void OnEntityManagerComplete()
		{
			_entityManagerReady = true;
			CheckManagerState();
		}

		private void CheckManagerState()
		{
			if(!_mapManagerReady || !_entityManagerReady) return;

			AfterTransitionComplete();

			_mapManagerReady = false;
			_entityManagerReady = false;
		}

		private void AfterTransitionComplete()
		{
			_mapManager.AfterTransitionComplete();
			_unifiedEntityManager.AfterTransitionComplete();
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("MapTransition", this);
			LoadTransitionInfo("transition.json");
			InitMap += InitMapProcess;
			TransitMap += TransitionProcess;
		}

		public override void _ExitTree()
		{
			InitMap -= InitMapProcess;
			TransitMap -= TransitionProcess;
			_mapManager.MapTransitionComplete -= OnMapManagerComplete;
			_unifiedEntityManager.EntityTransitionComplete -= OnEntityManagerComplete;
			GlobalObjectManager.RemoveGlobalObject("MapTransition");
		}

		public override void _Ready()
		{
			_mapManager = GetNode<MapManager>("MapManager");
			_unifiedEntityManager = GetNode<UnifiedEntityManager>("UnifiedEntityManager");

			_mapManager.MapTransitionComplete += OnMapManagerComplete;
			_unifiedEntityManager.EntityTransitionComplete += OnEntityManagerComplete;
		}
	}
}
