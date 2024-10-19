using Godot;
using MyGame.Component;
using MyGame.Entity;
using System.Collections.Generic;

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

    public partial class BaseEntityManager<T> : Node where T : Node2D, IEntity
    {
        protected readonly Dictionary<string, List<EntityInstanceInfo>> _globalEntityInfomation = new();

        protected readonly Dictionary<string, PackedScene> _entities = new();
        protected readonly List<T> _instances = new();

        private string _name;

        public BaseEntityManager()
        {
            _name = GetType().Name;
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

        protected void FreeLivingEntity(T entity)
        {
            _instances.Remove(entity);
            entity.QueueFree();
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

        protected T SpawnEntity(EntityInstanceInfo instanceInfo)
        {
            if (!_entities.ContainsKey(instanceInfo.EntityType))
            {
                LoadEntity(instanceInfo.EntityType);
            }
            T entity = _entities[instanceInfo.EntityType].Instantiate<T>();

            _instances.Add(entity);
            AddChild(entity);

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
                entity.SetPhysicsProcess(enable);
            }
        }

        protected void AddAllLivingEntitiesToRenderingOrderGroup(string groupName)
        {
            foreach (var entity in _instances)
            {
                GlobalObjectManager.IncludeNodeIntoRenderingOrderGroup(groupName, entity);
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
