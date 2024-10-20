using Godot;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public void ShowTip();
        public void HideTip();
        public void Interact(IInteractionParticipant participant);
    }
}
