using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public partial class DynamicEntity00 : BaseInteractableDynamicEntity
    {
        private class NormalState : IState
        {
            public void Enter(IEntity entity)
            {
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new InputNavigator();
                });
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new CharacterAnimationPlayer(((DynamicEntity00)entity).GetNode<AnimatedSprite2D>("AnimatedSprite2D"));
                });
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new FrictionVelocityAlgorithm(((DynamicEntity00)entity), 100, 2000, 1000);
                });
            }

            public void Exit(IEntity entity) { }

            public void HandleStateTransition(IEntity entity, string input)
            {
            }
        }
    }
}
