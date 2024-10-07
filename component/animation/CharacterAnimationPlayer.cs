using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class CharacterAnimationPlayer : IAnimationPlayer
    {
        private AnimatedSprite2D _animatedSprite2D;

        protected double _animationChangeCooldown = 0.1;

        private double _animationChangeTimer = 0;
        private bool _animationChangeReady = true;

        protected int[] _directionSuffix = { 6, 8, 4, 2 };
        protected Dictionary<int, (float min, float max)> _angleToDirectionSuffix = new()
        {
            { 6, (-45f, 45f) },
            { 8, (45f, 135f) },
            { 4, (135f, -135f) },
            { 2, (-135f, -45f) }
        };

        public CharacterAnimationPlayer(AnimatedSprite2D animatedSprite2D)
        {
            _animatedSprite2D = animatedSprite2D;
        }

        private void PlayAnimation(string animationName)
        {
            if (animationName == null) return;
            _animatedSprite2D.Play(animationName);
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

        private void UpdateAnimationChangeCooldownTimer(double delta)
        {
            if (_animationChangeReady) return;

            _animationChangeTimer += delta;
            if (_animationChangeTimer > _animationChangeCooldown)
            {
                _animationChangeReady = true;
                _animationChangeTimer = 0;
            }
        }

        private void ModifyAnimation(Vector2 direction)
        {
            if (!_animationChangeReady) return;

            string currentAnimationName = _animatedSprite2D.Animation;
            int currentDirectionSuffix = int.Parse(currentAnimationName[^1].ToString());

            if (direction.IsZeroApprox())
            {
                string newIdleAnimation = "idle-" + currentDirectionSuffix;
                PlayAnimation(newIdleAnimation);
            }
            else
            {
                int targetDirectionSuffix = GetTargetDirectionSuffix(direction);
                int nextDirectionSuffix = GetNextDirectionSuffix(currentDirectionSuffix, targetDirectionSuffix);
                PlayAnimation("run-" + nextDirectionSuffix);
            }

            _animationChangeReady = _animatedSprite2D.Animation == currentAnimationName;
        }

        public void UpdateAnimation(Vector2 direction, double delta)
        {
            UpdateAnimationChangeCooldownTimer(delta);
            ModifyAnimation(direction);
        }

        public void UpdateAnimationWithoutConstraint(Vector2 direction)
        {
            int nextAnimationSuffix = GetTargetDirectionSuffix(direction);
            if (direction.IsZeroApprox())
            {
                PlayAnimation("idle-" + nextAnimationSuffix);
            }
            else
            {
                PlayAnimation("run-" + nextAnimationSuffix);
            }
        }
    }
}
