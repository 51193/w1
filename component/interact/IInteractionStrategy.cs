using MyGame.Entity;

namespace MyGame.Component
{
    public interface IInteractionStrategy
    {
        public void Interaction(BaseInteractableDynamicEntity dynamicEntity);
        public string GetState();
        public void SetState(string state);
    }
}
