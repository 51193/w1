using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public partial class DynamicEntity00 : BaseInteractableDynamicEntity
    {
        private class HardwareInputControlState : IState
        {
            public void Enter(IEntity entity)
            {
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new InputNavigator();
                });
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args)
            {
                switch (input)
                {
                    case "GoStraight":
                        if (args.Length > 0 && args[0] is Vector2 position)
                        {
                            ((DynamicEntity00)entity)._stateManager.ChangeState("ControlState", new StraightForwardControlState(position));
                        }
                        break;
                }
            }
        }
        private class StraightForwardControlState : IState
        {
            private readonly Vector2 _targetPosition;
            private uint entityOriginalCollisionMask = 0;
            public StraightForwardControlState(Vector2 targetPosition)
            {
                _targetPosition = targetPosition;
            }
            public void Enter(IEntity entity)
            {
                entityOriginalCollisionMask = ((DynamicEntity00)entity).CollisionMask;
                ((DynamicEntity00)entity).CollisionMask = 0;
                ((DynamicEntity00)entity).IsTransitable = false;
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new StraightToTargetNavigator((DynamicEntity00)entity, _targetPosition, () =>
                    {
                        ((DynamicEntity00)entity)._stateManager.ChangeState("ControlState", new HardwareInputControlState());
                    });
                });
            }
            public void Exit(IEntity entity)
            {
                ((DynamicEntity00)entity).CollisionMask = entityOriginalCollisionMask;
                ((DynamicEntity00)entity).IsTransitable = true;
            }
            public void HandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
        private class NormalState : IState
        {
            public void Enter(IEntity entity)
            {
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new CharacterAnimationPlayer(((DynamicEntity00)entity)._animationSprite2DNode);
                });
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new FrictionVelocityAlgorithm(((DynamicEntity00)entity), 100, 2000, 1000);
                });
            }
            public void Exit(IEntity entity) { }
            public void HandleStateTransition(IEntity entity, string input, params object[] args) { }
        }
    }
}
