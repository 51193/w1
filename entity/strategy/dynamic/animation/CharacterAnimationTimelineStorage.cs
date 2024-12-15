using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class CharacterAnimationTimelineStorage : BasicStrategy<BasicCharacter>
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

        protected override void Activate(BasicCharacter entity, double dt = 0)
        {
            AnimationTimelineData timelineData = AccessData<AnimationTimelineData>(entity);
            if (timelineData.HaveInitialized == false)
            {
                entity.AnimationPlayerNode.Seek(timelineData.CurrentAnimationPosition, true);
                timelineData.HaveInitialized = true;
            }
            else
            {
                timelineData.CurrentAnimationPosition = entity.AnimationPlayerNode.CurrentAnimationPosition;
            }
        }
    }
}
