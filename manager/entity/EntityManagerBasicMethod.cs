using Godot;
using MyGame.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGame.Manager
{
    public partial class EntityManager: Node
    {
        public void ClearAllEntitiesFromMapRecord(string mapName)
        {
            if (GlobalEntityInstanceInfoDictionary.ContainsKey(mapName))
            {
                GlobalEntityInstanceInfoDictionary[mapName].Clear();
            }
        }

        protected void AddEntityToMapRecord(string mapName, EntityInstanceInfo instance)
        {
            if (!GlobalEntityInstanceInfoDictionary.ContainsKey(mapName))
            {
                GlobalEntityInstanceInfoDictionary[mapName] = new();
            }
            GlobalEntityInstanceInfoDictionary[mapName].Add(instance);
        }

        protected void FreeLivingEntity(IEntity entity)
        {
            _instances.Remove(entity);
            entity.GetNode().QueueFree();
        }

        public void RecordAllLivingEntitiesToMapRecord(string mapName)
        {
            foreach (var instance in _instances)
            {
                AddEntityToMapRecord(mapName, new EntityInstanceInfo(instance.EntityName, instance.SaveData()));
            }
        }

        public void ClearAllLivingEntities()
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
            if (_loadedEntities.ContainsKey(entityName))
            {
                GD.PrintErr($"Duplicate entity loaded: {entityName}");
            }
            else
            {
                _loadedEntities[entityName] = GlobalObjectManager.GetResource(entityName);
                GD.Print($"Entity loaded: {entityName}");
            }
        }

        protected IEntity SpawnEntity(EntityInstanceInfo instanceInfo)
        {
            if (!_loadedEntities.ContainsKey(instanceInfo.EntityType))
            {
                LoadEntity(instanceInfo.EntityType);
            }
            Node node = _loadedEntities[instanceInfo.EntityType].Instantiate();
            IEntity entity = node as IEntity;

            _instances.Add(entity);
            _entityYSorter.AddChild(entity.GetNode());

            entity.LoadData(instanceInfo.SaveHead);

            GD.Print($"Entity instantiated: {entity.EntityName}({entity.Position.X}, {entity.Position.Y})");
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
            if (GlobalEntityInstanceInfoDictionary.TryGetValue(mapName, out var entities))
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

        protected void CallAllLivingEntitiesInitiateProcess()
        {
            foreach (var entity in _instances)
            {
                entity.EntityInitiateProcess();
            }
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

        public override void _Ready()
        {
            _entityYSorter = GetNode<Node2D>("EntityYSorter");
        }
    }
}
