using MyGame.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Entity
{
    public abstract partial class StaticEntity : BaseStaticEntity
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
