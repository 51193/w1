using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public partial class EntityManager : Node
	{
		public Dictionary<string, List<EntityInstanceInfo>> GlobalEntityInstanceInfoDictionary = new();
		protected readonly Dictionary<string, PackedScene> _entities = new();
		protected readonly List<IEntity> _instances = new();

		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			CallAllLivingEntitiesInitiateProcess();
            AddAllLivingEntitiesToRenderingOrderGroup(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		private void SpawnEntityWithEntranceAnimation(EntityInstanceInfo instanceInfo, Vector2 toPosition)
		{
			IEntity entity = SpawnEntity(instanceInfo);
			entity.HandleStateTransition("ControlState", "GoStraight", toPosition);
		}

		public void OnMapChanged(IEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition)
		{
			if (currentMapName == null)
			{
				GD.PrintErr("Invalid current map, can't change map before initiate");
				return;
			}
			ClearAllLivinEntitiesRenderingOrderGroupName(currentMapName);
			GlobalObjectManager.ClearNodeInRenderingOrderGroup(currentMapName);

			ClearAllEntitiesFromMapRecord(currentMapName);

			string entityName = entity.GetEntityName();
			ISaveComponent save = entity.SaveData();
			save.SearchDataType<BaseSaveComponent>().Position = fromPosition;
            FreeLivingEntity(entity);

			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();

			SpawnEntityWithEntranceAnimation(new EntityInstanceInfo(entityName, save), ToPosition);

            SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
            AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);
            SetAllLivingEntitiesPhysicsProcess(false);
            CallAllLivingEntitiesInitiateProcess();

			ClearAllEntitiesFromMapRecord(nextMapName);
			RecordAllLivingEntitiesToMapRecord(nextMapName);

            EmitSignal(SignalName.EntityTransitionComplete);
            GD.Print($"Entities have swapped to {currentMapName}");
		}
	}
}
