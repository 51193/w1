using MyGame.Entity;

namespace MyGame.Component
{
    public interface IInteractionStrategy
    {
        public void Interaction(BaseInteractableDynamicEntity dynamicEntity);
    }
}
