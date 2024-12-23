using Godot;
using MyGame.Entity.MainBody;
using MyGame.Entity.Save;
using System.Collections.Generic;

namespace MyGame.Manager
{
    public partial class EntityManager : Node
    {
        [Export]
        public Node2D _entityYSorter;

        public Dictionary<string, List<EntityInstanceInfo>> GlobalEntityInstanceInfoDictionary = new();
        private readonly Dictionary<string, PackedScene> _loadedEntities = new();
        private readonly List<IEntity> _instances = new();

        [Signal]
        public delegate void EntityTransitionCompleteEventHandler();

        private void SpawnEntityWithEntranceAnimation(EntityInstanceInfo instanceInfo, Vector2 toPosition)
        {
            IEntity entity = SpawnEntity(instanceInfo);
            entity.StateManager.Transit("Move", "GoStraight", toPosition);
        }

        public void OnMapChanged(IEntity entity, string currentMapName, string nextMapName, Vector2 fromPosition, Vector2 ToPosition)
        {
            ClearAllEntitiesFromMapRecord(currentMapName);

            string entityName = entity.EntityName;
            ISaveComponent save = entity.SaveData();
            save.SearchDataType<BaseSaveComponent>().Position = fromPosition;
            FreeLivingEntity(entity);

            RecordAllLivingEntitiesToMapRecord(currentMapName);
            ClearAllLivingEntities();

            SpawnEntityWithEntranceAnimation(new EntityInstanceInfo(entityName, save), ToPosition);

            SpawnAllWaitingEntitiesFromMapRecord(nextMapName);
            SetAllLivingEntitiesPhysicsProcess(false);

            ClearAllEntitiesFromMapRecord(nextMapName);
            RecordAllLivingEntitiesToMapRecord(nextMapName);

            EmitSignal(SignalName.EntityTransitionComplete);
            GD.Print($"Entities have swapped to {currentMapName}");
        }

        public void OnMapFresh(string currentMapName)
        {
            ClearAllLivingEntities();
            SpawnAllWaitingEntitiesFromMapRecord(currentMapName);
            SetAllLivingEntitiesPhysicsProcess(false);

            EmitSignal(SignalName.EntityTransitionComplete);
            GD.Print($"Entities in {currentMapName} loaded");
        }
    }
}
