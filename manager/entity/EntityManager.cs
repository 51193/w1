using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public class EntityInstanceInfo
	{
		public string EntityType;
		public ISaveComponent SaveHead;

		public EntityInstanceInfo(string entityType, ISaveComponent saveHead)
		{
			EntityType = entityType;
			SaveHead = saveHead;
		}
	}

	public partial class EntityManager : Node
	{
		protected readonly Dictionary<string, List<EntityInstanceInfo>> _globalEntityInfomation = new();
		protected readonly Dictionary<string, PackedScene> _entities = new();
		protected readonly List<IEntity> _instances = new();

		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public EntityManager()
		{
			Init();
		}

		public void Init()
		{

			BaseSaveComponent baseSave = new()
			{
				Position = new Vector2(0, 0),
				States = null
			};

			CharacterSaveComponent characterSave = new()
			{
				TestText = "DynamicEntity0"
			};

			baseSave.Next = characterSave;

			_globalEntityInfomation["Map0"] = new()
			{
				new EntityInstanceInfo("DynamicEntity0", baseSave),

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

		protected void ClearAllEntitiesFromMapRecord(string mapName)
		{
			if (_globalEntityInfomation.ContainsKey(mapName))
			{
				_globalEntityInfomation[mapName].Clear();
			}
		}

		protected void AddEntityToMapRecord(string mapName, EntityInstanceInfo instance)
		{
			if (!_globalEntityInfomation.ContainsKey(mapName))
			{
				_globalEntityInfomation[mapName] = new();
			}
			_globalEntityInfomation[mapName].Add(instance);
		}

		protected void FreeLivingEntity(IEntity entity)
		{
			_instances.Remove(entity);
			entity.GetNode().QueueFree();
		}

		protected void RecordAllLivingEntitiesToMapRecord(string mapName)
		{
			foreach (var instance in _instances)
			{
				AddEntityToMapRecord(mapName, new EntityInstanceInfo(instance.GetEntityName(), instance.SaveData()));
			}
		}

		protected void ClearAllLivingEntities()
		{
			_instances.RemoveAll
				(i =>
				{
					i.GetNode().QueueFree();
					return true;
				});
		}

		protected void LoadEntity(string entityName)
		{
			if (_entities.ContainsKey(entityName))
			{
				GD.PrintErr($"Duplicate entity loaded: {entityName}");
			}
			else
			{
				_entities[entityName] = GlobalObjectManager.GetResource(entityName);
				GD.Print($"Entity loaded: {entityName}");
			}
		}

		protected IEntity SpawnEntity(EntityInstanceInfo instanceInfo)
		{
			if (!_entities.ContainsKey(instanceInfo.EntityType))
			{
				LoadEntity(instanceInfo.EntityType);
			}
			Node node = _entities[instanceInfo.EntityType].Instantiate();
			IEntity entity = node as IEntity;

			_instances.Add(entity);
			AddChild(entity.GetNode());

			entity.LoadData(instanceInfo.SaveHead);

			GD.Print($"Entity instantiated: {entity.GetEntityName()}({entity.Position.X}, {entity.Position.Y})");
			return entity;
		}

		protected void SpawnEntities(List<EntityInstanceInfo> entityInstances)
		{
			foreach (var entityInstance in entityInstances)
			{
				SpawnEntity(entityInstance);
			}
		}

		protected void SpawnAllWaitingEntitiesFromMapRecord(string mapName)
		{
			if (_globalEntityInfomation.TryGetValue(mapName, out var entities))
			{
				SpawnEntities(entities);
			}
		}

		protected void SetAllLivingEntitiesPhysicsProcess(bool enable)
		{
			foreach (var entity in _instances)
			{
				entity.GetNode().SetPhysicsProcess(enable);
			}
		}

		protected void AddAllLivingEntitiesToRenderingOrderGroup(string groupName)
		{
			foreach (var entity in _instances)
			{
				GlobalObjectManager.IncludeNodeIntoRenderingOrderGroup(groupName, entity.GetNode());
				entity.SetRenderingGroupName(groupName);
			}
		}

		protected void ClearAllLivinEntitiesRenderingOrderGroupName(string groupName)
		{
			foreach (var entity in _instances)
			{
				if (entity.GetRenderingGroupName() == groupName)
				{
					entity.SetRenderingGroupName(null);
				}
			}
		}

		protected void CallAllLivingEntitiesInitiateProcess()
		{
			foreach (var entity in _instances)
			{
				entity.EntityInitiateProcess();
			}
		}

		public void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
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
			SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
			SpawnEntityWithEntranceAnimation(new EntityInstanceInfo(entityName, save), ToPosition);
			AddAllLivingEntitiesToRenderingOrderGroup(nextMapName);
			CallAllLivingEntitiesInitiateProcess();
			SetAllLivingEntitiesPhysicsProcess(false);
			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Entities have swapped to {currentMapName}");
		}

		public async void AfterTransitionComplete()
		{
			await Task.Delay(1);
			SetAllLivingEntitiesPhysicsProcess(true);
			GD.Print($"Entities transition complete");
		}

		public override void _EnterTree()
		{
			GD.Print($"EntityManager enter");
		}

		public override void _ExitTree()
		{
			GD.Print($"EntityManager exit");
		}
	}
}
