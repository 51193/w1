using Godot;
using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public partial class BaseEntityManager<T> : Node where T : Node2D, IEntity
    {
        protected readonly Dictionary<string, Dictionary<string, List<Tuple<Vector2, string>>>> _globalEntityPosition = new();

        protected readonly Dictionary<string, PackedScene> _entities = new();
        protected readonly List<T> _instances = new();

        private string _name;

        public BaseEntityManager()
        {
            _name = GetType().Name;
        }

        protected void ClearAllEntitiesFromMapRecord(string mapName)
        {
            if (_globalEntityPosition.ContainsKey(mapName))
            {
                _globalEntityPosition[mapName].Clear();
            }
        }

        protected void AddEntityToMapRecord(string mapName, string entityName, Tuple<Vector2, string>infoTuple)
        {
            if (!_globalEntityPosition.ContainsKey(mapName))
            {
                _globalEntityPosition[mapName] = new();
            }
            if (!_globalEntityPosition[mapName].ContainsKey(entityName))
            {
                _globalEntityPosition[mapName][entityName] = new();
            }
            _globalEntityPosition[mapName][entityName].Add(infoTuple);
        }

        protected void FreeLivingEntity(T entity)
        {
            _instances.Remove(entity);
            entity.QueueFree();
        }

        protected void MoveLivingEntityToMapRecord(T entity, string mapName, Vector2 position)
        {
            AddEntityToMapRecord(mapName, entity.GetEntityName(), Tuple.Create(position, entity.GetState()));
            FreeLivingEntity(entity);
        }

        protected void RecordAllLivingEntitiesToMapRecord(string mapName)
        {
            foreach (var instance in _instances)
            {
                AddEntityToMapRecord(mapName, instance.GetEntityName(), Tuple.Create(instance.Position, instance.GetState()));
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

        protected T SpawnEntity(string entityName, Tuple<Vector2, string> info)
        {
            if (!_entities.ContainsKey(entityName))
            {
                LoadEntity(entityName);
            }
            T entity = _entities[entityName].Instantiate<T>();

            _instances.Add(entity);
            AddChild(entity);

            entity.Position = info.Item1;
            entity.SetState(info.Item2);

            GD.Print($"Entity instantiated: {entity.GetEntityName()}({entity.Position.X}, {entity.Position.Y})");
            return entity;
        }

        protected void SpawnEntities(string entityName, List<Tuple<Vector2, string>> infos)
        {
            foreach (var info in infos)
            {
                SpawnEntity(entityName, info);
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
