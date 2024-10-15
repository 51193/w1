using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public abstract partial class BaseCharacter : BaseInteractableDynamicEntity
    {
        public override ISaveComponent SaveData(ISaveComponent saveComponent = null)
        {
            CharacterSaveComponent save = new();
            save.SaveData(this);
            save.Next = saveComponent;
            return base.SaveData(save);
        }

        public override ISaveComponent LoadData(ISaveComponent saveComponent)
        {
            CharacterSaveComponent save = (CharacterSaveComponent)base.LoadData(saveComponent);
            save.LoadData(this);
            return save.Next;
        }
    }
}
