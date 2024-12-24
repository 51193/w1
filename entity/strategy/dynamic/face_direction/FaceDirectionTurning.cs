using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class FaceDirectionTurning : BasicStrategy<BasicCharacter>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(SimpleDirectionData),
                    typeof(SimpleFaceDirectionData),
                    typeof(TurningCircleRadiusData)
                };
            }
        }

        protected override void Activate(BasicCharacter entity, double dt = 0)
        {
            SimpleDirectionData directionData = AccessData<SimpleDirectionData>(entity);

            if (directionData.Direction.IsZeroApprox())
            {
                return;
            }

            SimpleFaceDirectionData faceDirectionData = AccessData<SimpleFaceDirectionData>(entity);
            TurningCircleRadiusData turningCircleRadiusData = AccessData<TurningCircleRadiusData>(entity); 

            float targetAngle = directionData.Direction.Angle();
            float currentAngle = faceDirectionData.FaceDirection;
            float turningRadius = turningCircleRadiusData.Radius * (float)dt;

            float angleDifference = targetAngle - currentAngle;
            angleDifference = Mathf.Wrap(angleDifference, -Mathf.Pi, Mathf.Pi);

            float resultAngle;
            if (Mathf.Abs(angleDifference) <= turningRadius)
            {
                resultAngle = targetAngle;
            }
            else
            {
                resultAngle = currentAngle + Mathf.Sign(angleDifference) * turningRadius;

                resultAngle = Mathf.Wrap(resultAngle, -Mathf.Pi, Mathf.Pi);
            }

            faceDirectionData.FaceDirection = resultAngle;
            GD.Print($"!{resultAngle}");
        }
    }
}
