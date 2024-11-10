using Godot;
using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Strategy
{
    public class FourFaceDirection : BasicStrategy<BasicCharacter>
    {
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
            entity.FaceDirectionTransitTimer += dt;
            if(entity.FaceDirectionTransitTimer > entity.FaceDirectionTransitCooldown)
            {
                entity.FaceDirectionTransitTimer = 0;
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
            if (!entity.Direction.IsZeroApprox() && IsReadyToTransitDirection(entity, dt))
            {
                entity.FaceDirection = GetNextDirectionSuffix(entity.FaceDirection, GetTargetDirectionSuffix(entity.Direction));
            }
        }
    }
}
