using MyGame.Entity;

namespace MyGame.Component
{
    public interface ISaveComponent
    {
        public ISaveComponent Next {  get; set; }
        public void SaveData(IEntity entity);
        public void LoadData(IEntity entity);
    }
}
