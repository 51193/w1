using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public partial class BaseInteractableStaticEntity : BaseStaticEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;
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

        public override string GetState()
        {
            if (_interactionPrompt == null) return null;
            return _interactionStrategy.Invoke(strategy => strategy.GetState());
        }

        public override void SetState(string newState)
        {
            _interactionStrategy.Invoke((strategy) => strategy.SetState(newState));
        }
    }
}
