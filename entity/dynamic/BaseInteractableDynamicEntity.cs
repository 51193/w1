using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public abstract partial class BaseInteractableDynamicEntity : DynamicEntity, IInteractableEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;

        public abstract HashSet<string> GetInteractableTags();

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            _interactionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public abstract void WhenParticipantIsNearest();

        public abstract void WhenParticipantIsNotNearest();

        public void Interact(IInteractionParticipant participant)
        {
            _interactionStrategy.Invoke(strategy => strategy.Interact(participant));
        }
    }
}
