using MyGame.Component;
using System;

namespace MyGame.Entity
{
    public abstract partial class BaseInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;
        public LazyLoader<IInteractionStrategy> InteractionStrategy { get { return _interactionStrategy; } }

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            _interactionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public abstract void ShowTip();

        public abstract void HideTip();
    }
}
