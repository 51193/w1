using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class FourFaceAnimation : BasicStrategy<BasicCharacter>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(SimpleAnimationNameData),
                    typeof(SimpleFaceDirectionData),
                };
            }
        }

        protected override void Activate(BasicCharacter entity, double dt = 0)
        {
            SimpleAnimationNameData animationNameData = AccessData<SimpleAnimationNameData>(entity);
            SimpleFaceDirectionData faceDirectionData = AccessData<SimpleFaceDirectionData>(entity);

            if (entity.Velocity.IsZeroApprox())
            {
                animationNameData.AnimationName = "idle";
            }
            else
            {
                animationNameData.AnimationName = "run";
            }
            string AnimationFullName = $"{animationNameData.AnimationName}-{faceDirectionData.FaceDirection}";
            entity.AnimatedSprite2DNode.Play(AnimationFullName);
        }
    }
}
