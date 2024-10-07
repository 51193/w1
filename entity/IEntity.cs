namespace MyGame.Entity
{
    public interface IEntity
    {
        public string GetRenderingGroupName();
        public void SetRenderingGroupName(string groupName);
        public string GetEntityName();
        public string GetState();
        public void SetState(string state);
        public void EntityInitiateProcess();
    }
}
