using MyGame.Component;

namespace MyGame.Entity
{
    public partial class BaseInteractableStaticEntity : BaseStaticEntity
    {
        protected LazyLoader<IInteractionStrategy> _interactionStrategy;

        public void Interact(BaseInteractableDynamicEntity dynamicEntity)
        {
            _interactionStrategy.Invoke(strategy => strategy.Interaction(this, dynamicEntity));
        }
    }
}
