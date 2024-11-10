using MyGame.Component;

namespace MyGame.Entity
{
    public abstract partial class BasicInteractableDynamicEntity : DynamicEntity, IInteractableEntity
    {
        public abstract void ShowTip();

        public abstract void HideTip();
    }
}
