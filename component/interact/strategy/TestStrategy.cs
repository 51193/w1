using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public class TestStrategy : IInteractionStrategy
    {
        public void Interaction(BaseInteractableDynamicEntity dynamicEntity)
        {

        }

        public string GetState()
        {
            return null;
        }


        public void SetState(string state)
        {

        }
    }
}
