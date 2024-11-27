using Godot;
using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class DoorAnimationTimelineStorage : BasicStrategy<DoorOpenable>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(AnimationTimelineData)
                };
            }
        }

        protected override void Activate(DoorOpenable entity, double dt = 0)
        {
            AnimationTimelineData timelineData = AccessData<AnimationTimelineData>(entity);
            if (timelineData.HaveInitialzed == false)
            {
                entity.AnimationPlayerNode.Seek(timelineData.CurrentAnimationPosition, true);
                timelineData.HaveInitialzed = true;
            }
            else
            {
                timelineData.CurrentAnimationPosition = entity.AnimationPlayerNode.CurrentAnimationPosition;
            }
        }
    }
}
