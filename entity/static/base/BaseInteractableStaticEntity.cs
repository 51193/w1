using MyGame.Component;
using System;

namespace MyGame.Entity
{
    public abstract partial class BaseInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        public LazyLoader<IInteractionStrategy> InteractionStrategy { get; set; }

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            InteractionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public abstract void ShowTip();

        public abstract void HideTip();
    }
}
