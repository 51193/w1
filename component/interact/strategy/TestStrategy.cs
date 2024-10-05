using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public class TestStrategy : IInteractionStrategy
    {
        public void Interaction(BaseInteractableStaticEntity staticEntity, BaseInteractableDynamicEntity dynamicEntity)
        {
            GD.Print($"{dynamicEntity.GetEntityName()} interact with {staticEntity.GetEntityName()}");
        }
    }
}
