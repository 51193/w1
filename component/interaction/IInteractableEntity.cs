using Godot;

namespace MyGame.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public void WhenParticipantIsNearest();
        public void WhenParticipantIsNotNearest();
        public void Interact(IInteractionParticipant participant);
    }
}
