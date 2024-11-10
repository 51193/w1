using Godot;
using MyGame.Entity;
using MyGame.Strategy;

namespace MyGame.Strategy
{
    public class DefaultVelocity : BasicStrategy<BasicDynamicEntity>
    {
        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            Vector2 velocity = entity.Velocity;
            Vector2 direction = entity.Direction;
            float maxVelocity = entity.MaxVelocity;
            float acceleration = entity.Acceleration;
            float friction = entity.Friction;

            if (acceleration < friction)
            {
                entity.Velocity = Vector2.Zero;
                return;
            }

            if (velocity.Length() < friction * (float)dt)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                velocity -= velocity.Normalized() * friction * (float)dt;
            }

            if (!direction.IsNormalized())
            {
                direction = direction.Normalized();
            }

            velocity += direction * acceleration * (float)dt;
            velocity *= (Mathf.Min(1, maxVelocity / velocity.Length()));
            entity.Velocity = velocity;
        }
    }
}
