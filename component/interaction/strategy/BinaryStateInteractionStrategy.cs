using MyGame.Entity;

namespace MyGame.Component
{
    public class BinaryStateInteractionStrategy : IInteractionStrategy
    {
        private readonly IInteractableEntity _entity;
        private readonly string _name;

        public BinaryStateInteractionStrategy(IInteractableEntity entity, string binaryStateName)
        {
            _entity = entity;
            _name = binaryStateName;
        }

        public bool IsInteractableWith(IInteractionParticipant participant)
        {
            return true;
        }

        public void Interact(IInteractionParticipant participant)
        {
            if (_entity is IEntity entity)
            {
                entity.HandleStateTransition(_name, null);
            }
        }
    }
}
