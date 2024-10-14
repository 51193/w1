using MyGame.Entity;

namespace MyGame.Component
{
    public interface IInteractionStrategy
    {
        public void Interact(IInteractionParticipant participant);
    }
}
