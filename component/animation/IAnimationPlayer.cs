using Godot;

namespace MyGame.Component
{
    public interface IAnimationPlayer
    {
        void PlayAnimation(string animationName);
        void UpdateAnimation(Vector2 directioin, double delta);
    }
}
