using Godot;
using System;

namespace MyGame.Component
{
    public class FrictionVelocityAlgorithm : IVelocityAlgorithm
    {
        public float MaxVelocity;
        public float Acceleration;
        public float Friction;

        public FrictionVelocityAlgorithm(float maxVelocity, float accelertion, float friction)
        {
            MaxVelocity = maxVelocity;
            Acceleration = accelertion;
            Friction = friction;
        }

        public Vector2 UpdateVelocity(Vector2 velocity, Vector2 direction, double delta)
        {
            if (Acceleration < Friction)
            {
                return Vector2.Zero;
            }

            if (velocity.Length() < Friction * (float)delta)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity -= velocity.Normalized() * Friction * (float)delta;
            }

            if (!direction.IsNormalized())
            {
                direction.Normalized();
            }

            velocity += direction * Acceleration * (float)delta;
            velocity *= (Math.Min(1, MaxVelocity / velocity.Length()));
            return velocity;
        }
    }
}
