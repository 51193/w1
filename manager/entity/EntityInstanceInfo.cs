using MyGame.Entity.Save;

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
}
