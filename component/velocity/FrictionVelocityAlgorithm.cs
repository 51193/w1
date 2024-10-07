using Godot;
using MyGame.Entity;
using System;

namespace MyGame.Component
{
    public class FrictionVelocityAlgorithm : IVelocityAlgorithm
    {
        private readonly BaseDynamicEntity _entity;
        private readonly float _maxVelocity;
        private readonly float _acceleration;
        private readonly float _friction;

        public FrictionVelocityAlgorithm(BaseDynamicEntity entity, float maxVelocity, float accelertion, float friction)
        {
            _entity = entity;
            _maxVelocity = maxVelocity;
            _acceleration = accelertion;
            _friction = friction;
        }

        public Vector2 UpdateVelocity(double delta)
        {
            Vector2 velocity = _entity.Velocity;
            Vector2 direction = _entity.Direction;
            if (_acceleration < _friction)
            {
                return Vector2.Zero;
            }

            if (velocity.Length() < _friction * (float)delta)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity -= velocity.Normalized() * _friction * (float)delta;
            }

            if (!direction.IsNormalized())
            {
                direction.Normalized();
            }

            velocity += direction * _acceleration * (float)delta;
            velocity *= (Math.Min(1, _maxVelocity / velocity.Length()));
            return velocity;
        }
    }
}
