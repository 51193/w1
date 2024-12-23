using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using MyGame.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
        [Export]
        private MapManager _mapManager;
        [Export]
        private EntityManager _entityManager;

        private string _currentMapName;

        private Dictionary<(string Departure, string Exit), TransitionInfo> _transitionsDict;

        private bool _mapManagerReady = false;
        private bool _entityManagerReady = false;

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

        public void InitMapProcess(string mapName)
        {
            _currentMapName = mapName;
            _mapManager.LoadMap(mapName);
            _entityManager.InitializeEntities(mapName);
        }

        public void InvokeManagers(string destinationName, string fromLandmarkName, string toLandmarkName, BasicDynamicEntity entity)
        {
            string currentMapName = _currentMapName;
            _currentMapName = destinationName;
            _mapManager.LoadMap(destinationName);
            Vector2 fromLandmarkPosition = _mapManager.GetLandmarkPosition(fromLandmarkName);
            Vector2 toLandmarkPosition = _mapManager.GetLandmarkPosition(toLandmarkName);
            _entityManager.OnMapChanged(entity, currentMapName, destinationName, fromLandmarkPosition, toLandmarkPosition);
        }

        public void TransitionProcess(string departureName, string exitName, Vector2 exitPosition, IEntity entity)
        {
            if (!_transitionsDict.TryGetValue((departureName, exitName), out var transition))
            {
                GD.PrintErr($"No transition found for: {departureName}-{exitName}");
            }
            entity.StateManager.Transit("Move", "GoStraight", exitPosition);

            entity.DataManager.Get<GoStraightData>().CallbackOnTargetReached.AddEvent(typeof(MapTransitionEvents), "InvokeManagers", transition);
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
            if (!_mapManagerReady || !_entityManagerReady) return;

            AfterTransitionComplete();

            _mapManagerReady = false;
            _entityManagerReady = false;
        }

        private void AfterTransitionComplete()
        {
            _mapManager.AfterTransitionComplete();
            _entityManager.AfterTransitionComplete();
        }

        public void ToSaveData(string filePath)
        {
            _entityManager.ClearAllEntitiesFromMapRecord(_currentMapName);
            _entityManager.RecordAllLivingEntitiesToMapRecord(_currentMapName);

            SaveData saveData = new()
            {
                CurrentMapName = _currentMapName,
                GlobalEntityInstanceInfo = _entityManager.GlobalEntityInstanceInfoDictionary
            };
            FileUtil.WriteToFile(filePath, JsonUtil.SerializeSaveData(saveData));
        }

        public void FromSaveData(string filePath)
        {
            SaveData saveData = JsonUtil.DeserializeSaveData(FileUtil.ReadFromFile(filePath));
            _currentMapName = saveData.CurrentMapName;
            _entityManager.GlobalEntityInstanceInfoDictionary = saveData.GlobalEntityInstanceInfo;
            _entityManager.OnMapFresh(saveData.CurrentMapName);
            _mapManager.LoadMap(saveData.CurrentMapName);
        }

        private static MapTransition _instance;
        public static MapTransition Instance
        {
            get
            {
                if (_instance == null)
                {
                    GD.PrintErr("MapTransition is not available");
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
                GD.PrintErr("Duplicate MapTransition entered the tree, this is not allowed");
            }
            LoadTransitionInfo("transition.json");
        }

        public override void _ExitTree()
        {
            _mapManager.MapTransitionComplete -= OnMapManagerComplete;
            _entityManager.EntityTransitionComplete -= OnEntityManagerComplete;
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public override void _Ready()
        {
            _mapManager.MapTransitionComplete += OnMapManagerComplete;
            _entityManager.EntityTransitionComplete += OnEntityManagerComplete;
        }
    }
}
