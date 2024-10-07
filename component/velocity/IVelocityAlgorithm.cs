using Godot;

namespace MyGame.Component
{
    public interface IVelocityAlgorithm
    {
        public Vector2 UpdateVelocity(double delta);
    }
}
