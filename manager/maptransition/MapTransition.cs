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
		private DynamicEntityManager _entityManager;

		private Dictionary<(string Departure, string Exit), TransitionInfo> _transitionsDict;

		private bool _mapManagerReady = false;
		private bool _entityManagerReady = false;

		[Signal]
		public delegate void InitMapEventHandler(string mapName);
		[Signal]
		public delegate void TransitMapEventHandler(string departureName, string exitName, Vector2 exitPosition, BaseDynamicEntity entity);
		[Signal]
		public delegate void TransitionCompleteEventHandler();

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
			_mapManager.EmitSignal(nameof(_mapManager.ChangeMap), mapName);
			_entityManager.EmitSignal(nameof(_entityManager.InitiateEntitiesOnMap), mapName);
		}

		private void InvokeManagers(string destinationName, string fromLandmarkName, string toLandmarkName, BaseDynamicEntity entity)
		{
			_mapManager.EmitSignal(nameof(_mapManager.ChangeMap), destinationName);
			Vector2 fromLandmarkPosition = _mapManager.GetLandmarkPosition(fromLandmarkName);
			Vector2 toLandmarkPosition = _mapManager.GetLandmarkPosition(toLandmarkName);
			_entityManager.EmitSignal(nameof(_entityManager.ExchangeEntityOnMap), entity, destinationName, fromLandmarkPosition, toLandmarkPosition);
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
			CheckTransitionState();
		}

		private void OnEntityManagerComplete()
		{
			_entityManagerReady = true;
			CheckTransitionState();
		}

		private void CheckTransitionState()
		{
			if(!_mapManagerReady || !_entityManagerReady) return;

			EmitSignal(SignalName.TransitionComplete);

			_mapManagerReady = false;
			_entityManagerReady = false;
		}

		private void AfterTransitionComplete()
		{
			_mapManager.AfterTransitionComplete();
			_entityManager.AfterTransitionComplete();
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("MapTransition", this);
			LoadTransitionInfo("transition.json");
			InitMap += InitMapProcess;
			TransitMap += TransitionProcess;
			TransitionComplete += AfterTransitionComplete;
		}

		public override void _ExitTree()
		{
			InitMap -= InitMapProcess;
			TransitMap -= TransitionProcess;
			TransitionComplete -= AfterTransitionComplete;
			GlobalObjectManager.RemoveGlobalObject("MapTransition");
		}

		public override void _Ready()
		{
			_mapManager = GetNode<MapManager>("MapManager");
			_entityManager = GetNode<DynamicEntityManager>("DynamicEntityManager");

			_mapManager.MapTransitionComplete += OnMapManagerComplete;
			_entityManager.EntityTransitionComplete += OnEntityManagerComplete;
		}
	}
}
