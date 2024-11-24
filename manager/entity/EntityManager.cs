using Godot;
using MyGame.Component;
using MyGame.Entity;
using MyGame.Entity.State;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class EntityManager : Node
	{
		[Export]
		public Node2D _entityYSorter;

		public Dictionary<string, List<EntityInstanceInfo>> GlobalEntityInstanceInfoDictionary = new();
		protected readonly Dictionary<string, PackedScene> _loadedEntities = new();
		protected readonly List<IEntity> _instances = new();

		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public EntityManager()
		{
			BaseSaveComponent head = new()
			{
				Position = new Vector2(0, 0),
				States = new Dictionary<string, Type>()
                {
					{ "General", typeof(PlayerDefaultState) },
					{ "Input", typeof(PlayerHardwareInputControlState) }
				},
				Data = new()
			};

			CharacterSaveComponent save = new()
			{
				ItemNameList = new List<string>() { "TestItem0" }
			};

			head.Next = save;

			BaseSaveComponent wallHead0 = new()
			{
				Position = new Vector2(-32, -16),
				States = new(),
				Data = new()
			};
			BaseSaveComponent wallHead1 = new()
			{
				Position = new Vector2(48, 0),
				States = new(),
				Data = new()
			};
            BaseSaveComponent doorHead1 = new()
            {
                Position = new Vector2(24, -16),
                States = new()
				{
					{"OpenState", typeof(DoorClosedState) }
				},
                Data = new()
            };

			GlobalEntityInstanceInfoDictionary["Map0"] = new List<EntityInstanceInfo>()
			{
				new("DynamicEntity0", head),
				new("Map0Wall0", wallHead0),
				new("Map0Wall1", wallHead1),
				new("DoorOpenable", doorHead1)
			};
		}

		public void InitializeEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		private void SpawnEntityWithEntranceAnimation(EntityInstanceInfo instanceInfo, Vector2 toPosition)
		{
			IEntity entity = SpawnEntity(instanceInfo);
			entity.StateManager.Transit("Input", "GoStraight", toPosition);
		}

		public void OnMapChanged(IEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition)
		{
			ClearAllEntitiesFromMapRecord(currentMapName);

			string entityName = entity.EntityName;
			ISaveComponent save = entity.SaveData();
			save.SearchDataType<BaseSaveComponent>().Position = fromPosition;
			FreeLivingEntity(entity);

			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();

			SpawnEntityWithEntranceAnimation(new EntityInstanceInfo(entityName, save), ToPosition);

			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SetAllLivingEntitiesPhysicsProcess(false);

			ClearAllEntitiesFromMapRecord(nextMapName);
			RecordAllLivingEntitiesToMapRecord(nextMapName);

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Entities have swapped to {currentMapName}");
		}

		public void OnMapFresh(string currentMapName)
		{
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(currentMapName);
			SetAllLivingEntitiesPhysicsProcess(false);

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Entities in {currentMapName} loaded");
		}
	}
}
