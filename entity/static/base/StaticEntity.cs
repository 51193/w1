using MyGame.Entity.Save;

namespace MyGame.Entity
{
    public abstract partial class StaticEntity : BasicStaticEntity
    {
        protected ISaveComponent HandleSaveData<T>(ISaveComponent saveComponent) where T : ISaveComponent, new()
        {
            T save = new();
            save.SaveData(this);
            save.Next = saveComponent;
            return base.SaveData(save);
        }

        protected ISaveComponent HandleLoadData<T>(ISaveComponent saveComponent) where T : ISaveComponent, new()
        {
            T save = (T)base.LoadData(saveComponent);
            save.LoadData(this);
            return save.Next;
        }
    }
}
