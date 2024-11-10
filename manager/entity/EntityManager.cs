using Godot;
using MyGame.Component;
using MyGame.Entity;
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
				States = new List<Type>() { typeof(CharacterDefaultState), typeof(CharacterHardwareInputControlState) }
			};

			CharacterSaveComponent save = new()
			{
				ItemNameList = new List<string>() { "TestItem0" }
			};

			head.Next = save;

			GlobalEntityInstanceInfoDictionary["Map0"] = new List<EntityInstanceInfo>()
			{
				new("DynamicEntity0", head)
			};
		}

		public void InitializeEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			CallAllLivingEntitiesInitializeProcess();
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		private void SpawnEntityWithEntranceAnimation(EntityInstanceInfo instanceInfo, Vector2 toPosition)
		{
			IEntity entity = SpawnEntity(instanceInfo);
			entity.StateManager.Transit<CharacterHardwareInputControlState>("GoStraight", toPosition, "OnReachedTarget");
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
			CallAllLivingEntitiesInitializeProcess();

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
			CallAllLivingEntitiesInitializeProcess();

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Entities in {currentMapName} loaded");
		}
	}
}
