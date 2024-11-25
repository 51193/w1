using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class InputDirection : BasicStrategy<BasicDynamicEntity>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>
                {
                    typeof(SimpleDirectionData)
                };
            }
        }

        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            Vector2 direction = Vector2.Zero;
            if (Input.IsActionPressed("move_right"))
            {
                direction.X += 1;
            }
            if (Input.IsActionPressed("move_left"))
            {
                direction.X -= 1;
            }
            if (Input.IsActionPressed("move_down"))
            {
                direction.Y += 1;
            }
            if (Input.IsActionPressed("move_up"))
            {
                direction.Y -= 1;
            }
            AccessData<SimpleDirectionData>(entity).Direction = direction.Normalized();
        }
    }
}
