using Godot;
using MyGame.Component;
using System;

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
                            ((DynamicEntity00)entity)._eventManager.RegistrateEvent("OnReachedTarget",
                                new Action(() =>
                                {
                                    ((DynamicEntity00)entity)._stateManager.ChangeState("ControlState", new HardwareInputControlState());
                                }));
                            ((DynamicEntity00)entity)._stateManager.ChangeState("ControlState", new StraightForwardControlState(position, "OnReachedTarget"));
                        }
                        break;
                }
            }
        }
        private class StraightForwardControlState : IState
        {
            private readonly Vector2 _targetPosition;
            private readonly string _callbackName;

            private uint _entityOriginalCollisionMask = 0;
            private bool _entityOriginalTransitableState = false;
            public StraightForwardControlState(Vector2 targetPosition, string callbackName)
            {
                _targetPosition = targetPosition;
                _callbackName = callbackName;
            }
            public void Enter(IEntity entity)
            {
                _entityOriginalCollisionMask = ((DynamicEntity00)entity).CollisionMask;
                _entityOriginalTransitableState = ((DynamicEntity00)entity).IsTransitable;
                ((DynamicEntity00)entity).CollisionMask = 0;
                ((DynamicEntity00)entity).IsTransitable = false;
                ((DynamicEntity00)entity).LoadStrategy(() =>
                {
                    return new StraightToTargetNavigator((DynamicEntity00)entity, _targetPosition, () =>
                    {
                        ((DynamicEntity00)entity)._eventManager.TriggerEvent(_callbackName);
                    });
                });
            }
            public void Exit(IEntity entity)
            {
                ((DynamicEntity00)entity).CollisionMask = _entityOriginalCollisionMask;
                ((DynamicEntity00)entity).IsTransitable = _entityOriginalTransitableState;
                ((DynamicEntity00)entity)._eventManager.UnregistrateEvent(_callbackName);
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
