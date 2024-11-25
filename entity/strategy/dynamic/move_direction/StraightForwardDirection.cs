using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using MyGame.Entity.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class StraightForwardDirection : BasicStrategy<BasicDynamicEntity>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(SimpleDirectionData),
                    typeof(GoStraightData)
                };
            }
        }

        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            Vector2 position = entity.Position;
            Vector2 target = AccessData<GoStraightData>(entity).TargetPosition;
            EventContainer callback = AccessData<GoStraightData>(entity).CallbackOnTargetReached;

            SimpleDirectionData directionData = AccessData<SimpleDirectionData>(entity);

            if ((target - position).Length() < 10)
            {
                directionData.Direction = Vector2.Zero;
                callback.ActivateEvents(entity);
            }
            else
            {
                directionData.Direction = (target - position).Normalized();
            }
        }
    }
}
