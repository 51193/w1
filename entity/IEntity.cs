namespace MyGame.Entity
{
    public interface IEntity
    {
        public string GetRenderingGroupName();

        public void SetRenderingGroupName(string groupName);

        public string GetEntityName();
    }
}
