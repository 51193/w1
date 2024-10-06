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
			_globalEntityPosition["Map00"] = new()
			{
				["Map0Wall0"] = new()
				{
					Tuple.Create(new Vector2(-32, -16), "")
				},
				["Map0Wall1"] = new()
				{
                    Tuple.Create(new Vector2(48, 0), "")
                },
				["InteractionTestEntity"] = new()
				{
                    Tuple.Create(new Vector2(24, -64), "")
                },
				["AnotherInteractionTestEntity"] = new()
				{
                    Tuple.Create(new Vector2(48, -64), "")
                },
				["DoorOpenable"] = new()
				{
                    Tuple.Create(new Vector2(24, -16), "")
				},
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
