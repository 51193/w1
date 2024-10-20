using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public abstract partial class BaseInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            _interactionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public abstract void ShowTip();

        public abstract void HideTip();

        public void Interact(IInteractionParticipant participant)
        {
            _interactionStrategy.Invoke(strategy => strategy.Interact(participant));
        }
    }
}
