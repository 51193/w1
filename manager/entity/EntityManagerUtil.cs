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
        /*
         * This type is different from other type when jsonfy and unjsonfy,
         * this is used in path.json to instantiate godot scene,
         * while others are used to instantiate object by reflection.
         */
        public string EntityType;
        public List<SaveComponentData> SaveNodeList;

        public EntityInstanceInfoData(string entityType, List<SaveComponentData> saveNodeList)
        {
            EntityType = entityType;
            SaveNodeList = saveNodeList;
        }
    }
}
