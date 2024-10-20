using Godot;
using MyGame.Component;
using MyGame.Entity;
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
			BaseSaveComponent baseSave = new()
			{
				Position = new Vector2(0, 0),
				States = null
			};

			CharacterSaveComponent characterSave = new()
			{
				TestText = "DynamicEntity00"
			};

			baseSave.Next = characterSave;

			_globalEntityInfomation["Map0"] = new()
			{
				new EntityInstanceInfo("DynamicEntity0", baseSave)
			};
		}

		private void SpawnEntityWithEntranceAnimation(EntityInstanceInfo instanceInfo, Vector2 toPosition)
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
			ISaveComponent save = entity.SaveData();
			save.SearchDataType<BaseSaveComponent>().Position = fromPosition;
			FreeLivingEntity(entity);
			RecordAllLivingEntitiesToMapRecord(currentMapName);
			ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SpawnEntityWithEntranceAnimation(new EntityInstanceInfo(entityName, save), ToPosition);
			AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);
			CallAllLivingEntitiesInitiateProcess();
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
