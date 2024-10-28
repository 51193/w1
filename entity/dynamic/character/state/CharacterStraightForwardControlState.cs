using Godot;
using MyGame.Component;
using MyGame.Util;

namespace MyGame.Entity
{
    public class CharacterStraightForwardControlState : IState
    {
        public Vector2 Position { get; set; }
        public string EventName { get; set; }

        private uint _entityOriginalCollisionMask = 0;
        private bool _entityOriginalTransitableState = false;
        public CharacterStraightForwardControlState() { }
        public CharacterStraightForwardControlState(Vector2 position, string eventName)
        {
            Position = position;
            EventName = eventName;
        }
        public void OnEnter(IEntity entity)
        {
            _entityOriginalCollisionMask = ((DynamicEntity0)entity).CollisionMask;
            _entityOriginalTransitableState = ((DynamicEntity0)entity).IsTransitable;
            ((DynamicEntity0)entity).CollisionMask = 0;
            ((DynamicEntity0)entity).IsTransitable = false;
            ((DynamicEntity0)entity).LoadStrategy(() =>
            {
                return new StraightForwardToTargetNavigator((BaseDynamicEntity)entity, Position, () =>
                {
                    entity.EventManager.TriggerEvent(EventName);
                });
            });
        }
        public void OnExit(IEntity entity)
        {
            ((DynamicEntity0)entity).CollisionMask = _entityOriginalCollisionMask;
            ((DynamicEntity0)entity).IsTransitable = _entityOriginalTransitableState;
        }
        public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
    }
}
