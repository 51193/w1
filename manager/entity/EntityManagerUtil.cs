using MyGame.Component;
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

    public class EntityInstanceInfoData
    {
        public string EntityType;
        public List<SaveComponentData> SaveNodesList;

        public EntityInstanceInfoData(string entityType, List<SaveComponentData> saveNodesList)
        {
            EntityType = entityType;
            SaveNodesList = saveNodesList;
        }
    }

    public class EntityData
    {
        public Dictionary<string, List<EntityInstanceInfoData>> GlobalEntityInfoData = new();
    }
}
