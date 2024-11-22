using Godot;
using MyGame.Entity;
using MyGame.Entity.Data;
using MyGame.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.Strategy
{
    public class DefaultVelocity : BasicStrategy<BasicDynamicEntity>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(SimpleDirectionData),
                    typeof(VelocityWithFrictionData)
                };
            }
        }

        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            SimpleDirectionData directionData = AccessData<SimpleDirectionData>(entity);
            VelocityWithFrictionData velocityWithFrictionData = AccessData<VelocityWithFrictionData>(entity);

            Vector2 velocity = entity.Velocity;
            Vector2 direction = directionData.Direction;
            float maxVelocity = velocityWithFrictionData.MaxVelocity;
            float acceleration = velocityWithFrictionData.Acceleration;
            float friction = velocityWithFrictionData.Friction;

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
