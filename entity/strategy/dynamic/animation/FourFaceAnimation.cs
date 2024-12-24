using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

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
            string suffix;
            float angle = faceDirectionData.FaceDirection;
            angle = Mathf.Wrap(angle, -Mathf.Pi, Mathf.Pi);

            if (angle >= -Mathf.Pi / 4 && angle < Mathf.Pi / 4)
            {
                suffix = "6";
            }
            else if (angle >= Mathf.Pi / 4 && angle < 3 * Mathf.Pi / 4)
            {
                suffix = "2";
            }
            else if (angle >= 3 * Mathf.Pi / 4 || angle < -3 * Mathf.Pi / 4)
            {
                suffix = "4";
            }
            else
            {
                suffix = "8";
            }

            string AnimationFullName = $"{animationNameData.AnimationName}-{suffix}";
            entity.AnimationPlayerNode.Play(AnimationFullName);
        }
    }
}
