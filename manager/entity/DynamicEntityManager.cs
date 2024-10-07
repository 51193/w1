using Godot;
using MyGame.Component;
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

		private static void OnNavigationFinished(BaseDynamicEntity entity, uint originalCollisionMask)
		{
            entity.CollisionMask = originalCollisionMask;
            entity.IsTransitable = true;
            entity.LoadStrategy(() =>
            {
                return new InputNavigator();
            });
        }

		private void SpawnEntityWithEntranceAnimation(string entityName, Vector2 fromPosition, Vector2 toPosition)
		{
			BaseDynamicEntity entity = SpawnEntity(entityName, Tuple.Create(fromPosition, ""));

			uint originalCollisionMask = entity.CollisionMask;

			entity.LoadStrategy(() =>
			{
				return new StraightToTargetNavigator(entity, toPosition, () =>
				{
					CallDeferred(nameof(OnNavigationFinished), entity, originalCollisionMask);
				});
			});
			entity.CollisionMask = 0;
			entity.IsTransitable = false;
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
			FreeLivingEntity(entity);
			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SpawnEntityWithEntranceAnimation(entityName, fromPosition, ToPosition);

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
