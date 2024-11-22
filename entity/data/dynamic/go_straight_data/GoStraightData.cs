using Godot;
using MyGame.Entity.Manager;

namespace MyGame.Entity.Data
{
    public class GoStraightData : BasicData
    {
        public Vector2 TargetPosition = Vector2.Zero;
        public EventContainer CallbackOnTargetReached = new();
    }
}
