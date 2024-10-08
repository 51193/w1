using Godot;
using MyGame.Component;
using System;

namespace MyGame.Entity
{
    public partial class BaseInteractableStaticEntity : BaseStaticEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;
        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            _interactionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }
        protected Label _interactionPrompt;

        public void Interact(BaseInteractableDynamicEntity dynamicEntity)
        {
            _interactionStrategy.Invoke(strategy => strategy.Interaction(dynamicEntity));
        }

        protected void InitInteractionPrompt(Label interactionLabel)
        {
            _interactionPrompt = interactionLabel;
            HideInteractionPrompt();
        }

        public void ShowInteractionPrompt()
        {
            if(_interactionPrompt == null) return;
            _interactionPrompt.Visible = true;
        }

        public void HideInteractionPrompt()
        {
            if (_interactionPrompt == null) return;
            _interactionPrompt.Visible = false;
        }
    }
}
