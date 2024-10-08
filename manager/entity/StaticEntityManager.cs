using Godot;
using MyGame.Manager;
using MyGame.Entity;
using System.Threading.Tasks;
using System;

namespace MyGame.Manager
{
	public partial class StaticEntityManager : BaseEntityManager<BaseStaticEntity>
	{
		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public StaticEntityManager()
		{
			Init();
		}

		private void Init()
		{
			_globalEntityInfomation["Map00"] = new()
			{
				new EntityInstance("Map0Wall0", new Vector2(-32, -16), null),
				new EntityInstance("Map0Wall1", new Vector2(48, 0), null),
				new EntityInstance("InteractionTestEntity", new Vector2(24, -64), null),
				new EntityInstance("AnotherInteractionTestEntity", new Vector2(48, -64), null),
				new EntityInstance("DoorOpenable", new Vector2(24, -16), null)
			};
		}

		public void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			AddAllLivingEntitiesToRenderingOrderGroup(mapName);
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
		}

		public void OnMapChanged(string currentMapName, string nextMapName)
		{
			if (currentMapName == null)
			{
				GD.PrintErr("Invalid current map, can't change map before initiate");
				return;
			}

			ClearAllLivinEntitiesRenderingOrderGroupName(currentMapName);

			ClearAllEntitiesFromMapRecord(currentMapName);
			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);

			AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);

			UpdateAllLivingEntitiesOnce();

			SetAllLivingEntitiesPhysicsProcess(false);

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Static entities have swapped to {nextMapName}");
		}

		public async void AfterTransitionComplete()
		{
			await Task.Delay(1);
			SetAllLivingEntitiesPhysicsProcess(true);
			GD.Print($"Static entities transition complete");
		}
	}
}
