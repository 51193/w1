using Godot;
using MyGame.Entity;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public partial class DynamicEntityManager: BaseEntityManager<BaseDynamicEntity>
	{
		[Signal]
		public delegate void InitiateEntitiesOnMapEventHandler(string mapName);
		[Signal]
		public delegate void ExchangeEntityOnMapEventHandler(BaseDynamicEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition);
		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public DynamicEntityManager()
		{
			Init();
			_name = "DynamicEntityManager";
		}

		private void Init()
		{
			_globalEntityPosition["Map00"] = new()
			{
				["DynamicEntity00"] = new()
			{
				new Vector2(0, 0)
			},
				["DynamicEntity01"] = new()
			{
				new Vector2(50, 50)
			}
			};
		}

		private void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			AddAllLivingEntitiesToRenderingOrderGroup(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		private async void SpawnEntityWithEntranceAnimation(string entityName, Vector2 fromPosition, Vector2 toPosition)
		{
			BaseDynamicEntity entity = SpawnEntity(entityName, fromPosition);
			uint originalCollisionMask = entity.CollisionMask;

			entity.SetTookOverPosition((fromPosition - toPosition).Length() * 2, toPosition);
			entity.CollisionMask = 0;
			entity.IsTransitable = false;

			await Task.Delay(500);

			entity.DisableTookOver();
			entity.CollisionMask = originalCollisionMask;
			entity.IsTransitable = true;
		}

		private void OnMapChanged(BaseDynamicEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition)
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

		public override void _EnterTree()
		{
			base._EnterTree();
			InitiateEntitiesOnMap += InitiateEntities;
			ExchangeEntityOnMap += OnMapChanged;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			InitiateEntitiesOnMap -= InitiateEntities;
			ExchangeEntityOnMap -= OnMapChanged;
		}
	}
}
