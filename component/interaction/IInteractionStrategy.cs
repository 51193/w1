namespace MyGame.Component
{
    public interface IInteractionStrategy
    {
        public bool IsInteractableWith(IInteractionParticipant participant);

        public void Interact(IInteractionParticipant participant);
    }
}
