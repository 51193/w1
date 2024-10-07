using Godot;

namespace MyGame.Component
{
    public interface IAnimationPlayer
    {
        public void UpdateAnimation(Vector2 direction, double delta);
        public void UpdateAnimationWithoutConstraint(Vector2 direction);
    }
}
