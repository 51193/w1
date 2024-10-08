using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public partial class DynamicEntityManager: BaseEntityManager<BaseDynamicEntity>
	{
		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public DynamicEntityManager()
		{
			Init();
		}

		private void Init()
		{
			_globalEntityInfomation["Map00"] = new()
			{
				new EntityInstance("DynamicEntity00", new Vector2(0, 0), null)
			};
		}

		private void SpawnEntityWithEntranceAnimation(EntityInstance instanceInfo, Vector2 toPosition)
		{
			BaseDynamicEntity entity = SpawnEntity(instanceInfo);
			entity.HandleStateTransition("ControlState", "GoStraight", toPosition);
		}

		public void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			AddAllLivingEntitiesToRenderingOrderGroup(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		public void OnMapChanged(BaseDynamicEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition)
		{
			if (currentMapName == null)
			{
				GD.PrintErr("Invalid current map, can't change map before initiate");
				return;
			}

			ClearAllLivinEntitiesRenderingOrderGroupName(currentMapName);

			ClearAllEntitiesFromMapRecord(currentMapName);
			string entityName = entity.GetEntityName();
			Dictionary<string, IState> entityStates = entity.GetStates();
			FreeLivingEntity(entity);
			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SpawnEntityWithEntranceAnimation(new EntityInstance(entityName, fromPosition, entityStates), ToPosition);

			AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);

			UpdateAllLivingEntitiesOnce();

			SetAllLivingEntitiesPhysicsProcess(false);

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Dynamic entities have swapped to {currentMapName}");
		}

		public async void AfterTransitionComplete()
		{
			await Task.Delay(1);
			SetAllLivingEntitiesPhysicsProcess(true);
			GD.Print($"Dynamic entities transition complete");
		}
	}
}
