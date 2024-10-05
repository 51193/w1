using MyGame.Entity;

namespace MyGame.Component
{
    public interface IInteractionStrategy
    {
        public void Interaction(BaseInteractableStaticEntity staticEntity, BaseInteractableDynamicEntity dynamicEntity);
    }
}
