using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Threading.Tasks;

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
			_globalEntityInfomation["Map0"] = new()
			{
				new EntityInstanceInfo("Map0Wall0",
					new BaseSaveComponent()
					{
						Position = new Vector2(-32, -16),
						States = null
					}),
				new EntityInstanceInfo("Map0Wall1",
					new BaseSaveComponent()
					{
						Position = new Vector2(48, 0),
						States = null
					}),
				new EntityInstanceInfo("DoorOpenable",
					new BaseSaveComponent()
					{
						Position = new Vector2(24, -16),
						States = null
					})
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
			CallAllLivingEntitiesInitiateProcess();
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
