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

    public class SaveComponentNodeData
    {
        public string Type;
        public Dictionary<string, object> Properties;

        public SaveComponentNodeData(string type, Dictionary<string, object> properties)
        {
            Type = type;
            Properties = properties;
        }
    }

    public class EntityInstanceInfoData
    {
        public string EntityType;
        public List<SaveComponentNodeData> SaveNodesList;

        public EntityInstanceInfoData(string entityType, List<SaveComponentNodeData> saveNodesList)
        {
            EntityType = entityType;
            SaveNodesList = saveNodesList;
        }
    }

    public class EntitySaveData
    {
        public Dictionary<string, List<EntityInstanceInfoData>> GlobalEntityInfoData = new();
    }
}
