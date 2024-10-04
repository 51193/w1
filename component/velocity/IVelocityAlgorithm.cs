using Godot;

namespace MyGame.Component
{
    public interface IVelocityAlgorithm
    {
        public Vector2 UpdateVelocity(Vector2 velocity, Vector2 direction, double delta);
    }
}
