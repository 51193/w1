using Godot;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public void ShowTips();
        public void HideTips();
        public void Interact(IInteractionParticipant participant);
    }
}
