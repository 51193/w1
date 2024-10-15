using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public abstract partial class BaseCharacter : BaseInteractableDynamicEntity
    {
        public override ISaveComponent SaveData(ISaveComponent saveComponent = null)
        {
            return HandleSaveData<CharacterSaveComponent>(saveComponent);
        }

        public override ISaveComponent LoadData(ISaveComponent saveComponent)
        {
            return HandleLoadData<CharacterSaveComponent>(saveComponent);
        }
    }
}
