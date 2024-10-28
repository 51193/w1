using System.Collections.Generic;

namespace MyGame.Manager
{
    public class SaveData
    {
        public string CurrentMapName { get; set; }
        public Dictionary<string, List<EntityInstanceInfo>> GlobalEntityInstanceInfo { get; set; }
    }
}
