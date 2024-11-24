using Godot;
using MyGame.Entity.Manager;

namespace MyGame.Entity.Data
{
    public class GoStraightData : BasicData
    {
        public Vector2 TargetPosition { get; set; } = Vector2.Zero;
        public EventContainer CallbackOnTargetReached { get; set; } = new();
    }
}
