using Godot;
using MyGame.Entity;
using System;
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
			_globalEntityPosition["Map00"] = new()
			{
				["DynamicEntity00"] = new()
			{
				Tuple.Create(new Vector2(0, 0), "")
			}
			};
		}

		private async void SpawnEntityWithEntranceAnimation(string entityName, Vector2 fromPosition, Vector2 toPosition, string animationToPlayForNewSpawnEntity)
		{
			BaseDynamicEntity entity = SpawnEntity(entityName, Tuple.Create(fromPosition, ""));

			entity.PlayAnimation(animationToPlayForNewSpawnEntity);
			
			uint originalCollisionMask = entity.CollisionMask;

			entity.SetTookOverPosition((fromPosition - toPosition).Length() * 2, toPosition);
			entity.CollisionMask = 0;
			entity.IsTransitable = false;

			await Task.Delay(500);

			entity.DisableTookOver();
			entity.CollisionMask = originalCollisionMask;
			entity.IsTransitable = true;
		}

		public void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			AddAllLivingEntitiesToRenderingOrderGroup(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		public void OnMapChanged(BaseDynamicEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition, string animationToPlayForNewSpawnEntity)
		{
			if (currentMapName == null)
			{
				GD.PrintErr("Invalid current map, can't change map before initiate");
				return;
			}

			ClearAllLivinEntitiesRenderingOrderGroupName(currentMapName);

			ClearAllEntitiesFromMapRecord(currentMapName);
			string entityName = entity.GetEntityName();
			FreeLivingEntity(entity);
			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SpawnEntityWithEntranceAnimation(entityName, fromPosition, ToPosition, animationToPlayForNewSpawnEntity);

			AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);

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
