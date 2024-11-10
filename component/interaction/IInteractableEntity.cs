using Godot;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public void ShowTip();
        public void HideTip();
        public bool IsInteractableWith(IInteractionParticipant participant)
        {
            return true;
        }
        public void Interact(IInteractionParticipant participant)
        {
        }
    }
}
