using Godot;
using MyGame.Entity;
using System;

namespace MyGame.Component
{
    public class StraightToTargetNavigator : INavigator
    {
        private readonly BaseDynamicEntity _entity;
        private readonly Vector2 _targetPosition;
        private readonly Action _onTargetReached;

        public StraightToTargetNavigator(BaseDynamicEntity entity, Vector2 targetPosition, Action onTargetReached = null)
        {
            _entity = entity;
            _targetPosition = targetPosition;
            _onTargetReached = onTargetReached;
        }

        public Vector2 UpdateDirection()
        {
            Vector2 position = _entity.Position;
            if ((_targetPosition - position).Length() < 10)
            {
                _onTargetReached?.Invoke();
                return Vector2.Zero;
            }
            else
            {
                return (_targetPosition - position).Normalized();
            }
        }
    }
}
