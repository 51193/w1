using MyGame.Component;
using System;

namespace MyGame.Entity
{
    public abstract partial class BaseInteractableDynamicEntity : DynamicEntity, IInteractableEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            _interactionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public abstract void ShowTips();

        public abstract void HideTips();

        public void Interact(IInteractionParticipant participant)
        {
            _interactionStrategy.Invoke(strategy => strategy.Interact(participant));
        }
    }
}
