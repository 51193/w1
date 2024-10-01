using Godot;
using MyGame.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Manager
{
	public partial class EntityManager : Node
	{
		private string _currentMapName;
		private readonly Dictionary<string, Dictionary<string, List<Vector2>>> _globalEntityPosition = new();

		private readonly Dictionary<string, PackedScene> _entities = new();
		private readonly List<BaseEntity> _instances = new();

		[Signal]
		public delegate void InitiateEntitiesOnMapEventHandler(string mapName);
		[Signal]
		public delegate void ExchangeEntityOnMapEventHandler(BaseEntity entity, string mapName, Vector2 fromPosition, Vector2 toPosition);
		[Signal]
		public delegate void EntityTransitionCompleteEventHandler();

		public EntityManager()
		{
			Init();
		}

		private void Init()
		{
			_globalEntityPosition["Map00"] = new()
            {
                ["Entity00"] = new()
            {
                new Vector2(0, 0)
            },
                ["Entity01"] = new()
            {
                new Vector2(50, 50)
            }
            };
		}

		private void InitiateEntities(string mapName)
		{
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			_currentMapName = mapName;
            AddAllLivingEntitiesToRenderingOrderSortGroup(_currentMapName);
            SetAllLivingEntitiesPhysicsProcess(false);
            EmitSignal(SignalName.EntityTransitionComplete);
        }

		private void ClearAllEntitiesFromMapRecord(string mapName)
		{
			if (_globalEntityPosition.ContainsKey(mapName))
			{
				_globalEntityPosition[mapName].Clear();
			}
		}

		private void AddEntityToMapRecord(string mapName, string entityName, Vector2 position)
        {
            if (!_globalEntityPosition.ContainsKey(mapName))
            {
                _globalEntityPosition[mapName] = new Dictionary<string, List<Vector2>>();
            }
            if (!_globalEntityPosition[mapName].ContainsKey(entityName))
            {
                _globalEntityPosition[mapName][entityName] = new List<Vector2>();
            }
            _globalEntityPosition[mapName][entityName].Add(position);
        }

		private void FreeLivingEntity(BaseEntity entity)
		{
			_instances.Remove(entity);
			entity.QueueFree();
		}

        private void MoveLivingEntityToMapRecord(BaseEntity entity, string mapName, Vector2 position)
        {
            AddEntityToMapRecord(mapName, entity.GetEntityName(), position);
			FreeLivingEntity(entity);
        }

		private void RecordAllLivingEntitiesToMapRecord(string mapName)
		{
			foreach(var instance in _instances)
			{
                AddEntityToMapRecord(mapName, instance.GetEntityName(), instance.Position);
            }
		}

		private void ClearAllLivingEntities()
		{
			_instances.RemoveAll
				(i =>
				{
					i.QueueFree();
					return true;
				});
		}

        private void LoadEntity(string entityName)
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

		private BaseEntity SpawnEntity(string entityName, Vector2 position)
		{
			if (!_entities.ContainsKey(entityName))
			{
				LoadEntity(entityName);
			}
			BaseEntity entity = _entities[entityName].Instantiate<BaseEntity>();

			_instances.Add(entity);
			AddChild(entity);
			entity.Position = position;
			GlobalObjectManager.EmitIncludeNodeIntoRenderingOrderGroupSignal(_currentMapName, entity);
            GD.Print($"Entity instantiated: {entity.GetEntityName()}({entity.Position.X}, {entity.Position.Y})");
            return entity;
		}

        private void SpawnEntities(string entityName, List<Vector2> positions)
        {
            foreach (var position in positions)
            {
				SpawnEntity(entityName, position);
            }
        }
        private async void SpawnEntityWithEntranceAnimation(string entityName, Vector2 fromPosition, Vector2 toPosition)
        {
			BaseEntity entity = SpawnEntity(entityName, fromPosition);
			uint originalCollisionMask = entity.CollisionMask;

			entity.SetTookOverPosition((fromPosition - toPosition).Length() * 2, toPosition);
			entity.CollisionMask = 0;
			entity.IsTransitable = false;

			await Task.Delay(500);

			entity.DisableTookOver();
			entity.CollisionMask = originalCollisionMask;
			entity.IsTransitable = true;
        }

        private void SpawnAllWaitingEntitiesFromMapRecord(string mapName)
        {
            if (_globalEntityPosition.TryGetValue(mapName, out var entities))
            {
                foreach (var e in entities)
                {
                    SpawnEntities(e.Key, e.Value);
                }
            }
        }

		private void SetAllLivingEntitiesPhysicsProcess(bool enable)
		{
			foreach(var entity in _instances)
			{
				entity.SetPhysicsProcess(enable);
			}
		}

		private void AddAllLivingEntitiesToRenderingOrderSortGroup(string groupName)
		{
			foreach(var entity in _instances)
			{
				GlobalObjectManager.EmitIncludeNodeIntoRenderingOrderGroupSignal(groupName, entity);
				entity.RenderingOrderGroupName = groupName;
			}
		}

		private void OnMapChanged(BaseEntity entity, string mapName, Vector2 fromPosition, Vector2 ToPosition)
		{
			if (_currentMapName == null)
			{
				GD.PrintErr("Invalid current map, can't change map before initiate");
				return;
			}

            GlobalObjectManager.EmitClearNodeFromRenderingOrderGroupSignal(_currentMapName);

            ClearAllEntitiesFromMapRecord(_currentMapName);
			string entityName = entity.GetEntityName();
			FreeLivingEntity(entity);
			RecordAllLivingEntitiesToMapRecord(_currentMapName);
            _currentMapName = mapName;
            ClearAllLivingEntities();
			SpawnAllWaitingEntitiesFromMapRecord(mapName);
			SpawnEntityWithEntranceAnimation(entityName, fromPosition, ToPosition);

			AddAllLivingEntitiesToRenderingOrderSortGroup(_currentMapName);

			SetAllLivingEntitiesPhysicsProcess(false);

			EmitSignal(SignalName.EntityTransitionComplete);
			GD.Print($"Entities have swapped to {_currentMapName}");
		}

		public async void AfterTransitionComplete()
		{
			await Task.Delay(1);
			SetAllLivingEntitiesPhysicsProcess(true);
			GD.Print($"Entity transit to {_currentMapName} complete");
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("EntityManager", this);
            InitiateEntitiesOnMap += InitiateEntities;
			ExchangeEntityOnMap += OnMapChanged;
		}

		public override void _ExitTree()
		{
			InitiateEntitiesOnMap -= InitiateEntities;
			ExchangeEntityOnMap -= OnMapChanged;
			GlobalObjectManager.RemoveGlobalObject("EntityManager");
		}
	}
}
