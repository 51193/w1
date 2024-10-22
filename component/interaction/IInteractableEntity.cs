using Godot;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public LazyLoader<IInteractionStrategy> InteractionStrategy { get; set; }
        public void ShowTip();
        public void HideTip();
        public bool IsInteractableWith(IInteractionParticipant participant)
        {
            return InteractionStrategy.Invoke(strategy => strategy.IsInteractableWith(participant));
        }
        public void Interact(IInteractionParticipant participant)
        {
            InteractionStrategy.Invoke(strategy => strategy.Interact(participant));
        }
    }
}
