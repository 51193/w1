using Godot;
using MyGame.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Manager
{
    public partial class BaseEntityManager<T>: Node where T: Node2D, IEntity
    {
        protected readonly Dictionary<string, Dictionary<string, List<Vector2>>> _globalEntityPosition = new();

        protected readonly Dictionary<string, PackedScene> _entities = new();
        protected readonly List<T> _instances = new();

        protected string _name = "BaseEntityManager(shouldn't display)";

        protected void ClearAllEntitiesFromMapRecord(string mapName)
        {
            if (_globalEntityPosition.ContainsKey(mapName))
            {
                _globalEntityPosition[mapName].Clear();
            }
        }

        protected void AddEntityToMapRecord(string mapName, string entityName, Vector2 position)
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

        protected void FreeLivingEntity(T entity)
        {
            _instances.Remove(entity);
            entity.QueueFree();
        }

        protected void MoveLivingEntityToMapRecord(T entity, string mapName, Vector2 position)
        {
            AddEntityToMapRecord(mapName, entity.GetEntityName(), position);
            FreeLivingEntity(entity);
        }

        protected void RecordAllLivingEntitiesToMapRecord(string mapName)
        {
            foreach (var instance in _instances)
            {
                AddEntityToMapRecord(mapName, instance.GetEntityName(), instance.Position);
            }
        }

        protected void ClearAllLivingEntities()
        {
            _instances.RemoveAll
                (i =>
                {
                    i.QueueFree();
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

        protected T SpawnEntity(string entityName, Vector2 position)
        {
            if (!_entities.ContainsKey(entityName))
            {
                LoadEntity(entityName);
            }
            T entity = _entities[entityName].Instantiate<T>();

            _instances.Add(entity);
            AddChild(entity);
            entity.Position = position;
            GD.Print($"Entity instantiated: {entity.GetEntityName()}({entity.Position.X}, {entity.Position.Y})");
            return entity;
        }

        protected void SpawnEntities(string entityName, List<Vector2> positions)
        {
            foreach (var position in positions)
            {
                SpawnEntity(entityName, position);
            }
        }

        protected void SpawnAllWaitingEntitiesFromMapRecord(string mapName)
        {
            if (_globalEntityPosition.TryGetValue(mapName, out var entities))
            {
                foreach (var e in entities)
                {
                    SpawnEntities(e.Key, e.Value);
                }
            }
        }

        protected void SetAllLivingEntitiesPhysicsProcess(bool enable)
        {
            foreach (var entity in _instances)
            {
                entity.SetPhysicsProcess(enable);
            }
        }

        protected void AddAllLivingEntitiesToRenderingOrderGroup(string groupName)
        {
            foreach (var entity in _instances)
            {
                GlobalObjectManager.EmitIncludeNodeIntoRenderingOrderGroupSignal(groupName, entity);
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

        public override void _EnterTree()
        {
            GD.Print($"EntityManager enter: {_name}");
        }

        public override void _ExitTree()
        {
            GD.Print($"EntityManager exit: {_name}");
        }
    }
}
