using Godot;
using MyGame.Entity.Data;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Strategy
{
    public class FourFaceDirection : BasicStrategy<BasicCharacter>
    {
        public override List<Type> DataNeeded
        {
            get
            {
                return new List<Type>()
                {
                    typeof(SimpleDirectionData),
                    typeof(SimpleFaceDirectionData),
                    typeof(FaceDirectionTimerData)
                };
            }
        }

        private readonly int[] _directionSuffix = { 6, 8, 4, 2 };
        private readonly Dictionary<int, (float min, float max)> _angleToDirectionSuffix = new()
        {
            { 6, (-45f, 45f) },
            { 8, (45f, 135f) },
            { 4, (135f, -135f) },
            { 2, (-135f, -45f) }
        };

        private bool IsReadyToTransitDirection(BasicCharacter entity, double dt)
        {
            FaceDirectionTimerData timerData = AccessData<FaceDirectionTimerData>(entity);

            timerData.FaceDirectionTransitTimer += dt;
            if (timerData.FaceDirectionTransitTimer > timerData.FaceDirectionTransitCooldown)
            {
                timerData.FaceDirectionTransitTimer -= timerData.FaceDirectionTransitCooldown;
                return true;
            }
            else
            {
                return false;
            }
        }

        private int GetTargetDirectionSuffix(Vector2 direction)
        {
            float angle = Mathf.Atan2(-direction.Y, direction.X);
            angle = Mathf.RadToDeg(angle);

            int targetDirectionSuffix = 2;
            foreach (var entry in _angleToDirectionSuffix)
            {
                var (min, max) = entry.Value;
                if ((min <= angle && angle < max) || (min > max && (angle >= min || angle < max)))
                {
                    targetDirectionSuffix = entry.Key;
                    break;
                }
            }

            return targetDirectionSuffix;
        }

        private int GetNextDirectionSuffix(int currentDirectionSuffix, int targetDirectionSuffix)
        {
            if (currentDirectionSuffix == targetDirectionSuffix) { return currentDirectionSuffix; }

            int currentDirectionSuffixIndex = Array.IndexOf(_directionSuffix, currentDirectionSuffix);
            int targetDirectionSuffixIndex = Array.IndexOf(_directionSuffix, targetDirectionSuffix);

            int clockwiseDistance = (targetDirectionSuffixIndex - currentDirectionSuffixIndex + _directionSuffix.Length) % _directionSuffix.Length;
            int counterclockwiseDistance = (currentDirectionSuffixIndex - targetDirectionSuffixIndex + _directionSuffix.Length) % _directionSuffix.Length;

            if (clockwiseDistance <= counterclockwiseDistance)
            {
                return _directionSuffix[(currentDirectionSuffixIndex + 1) % _directionSuffix.Length];
            }
            else
            {
                return _directionSuffix[(currentDirectionSuffixIndex - 1 + _directionSuffix.Length) % _directionSuffix.Length];
            }
        }

        protected override void Activate(BasicCharacter entity, double dt = 0)
        {
            SimpleDirectionData directionData = AccessData<SimpleDirectionData>(entity);
            SimpleFaceDirectionData faceDirectionData = AccessData<SimpleFaceDirectionData>(entity);

            if (!directionData.Direction.IsZeroApprox() && IsReadyToTransitDirection(entity, dt))
            {
                faceDirectionData.FaceDirection = GetNextDirectionSuffix(faceDirectionData.FaceDirection, GetTargetDirectionSuffix(directionData.Direction));
            }
        }
    }
}
