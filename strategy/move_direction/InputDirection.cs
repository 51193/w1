using Godot;
using MyGame.Entity;

namespace MyGame.Strategy
{
    public class InputDirection : BasicStrategy<BasicDynamicEntity>
    {
        protected override void Activate(BasicDynamicEntity entity, double dt = 0)
        {
            Vector2 direction = Vector2.Zero;
            if (Input.IsActionPressed("move_right"))
            {
                direction.X += 1;
            }
            if (Input.IsActionPressed("move_left"))
            {
                direction.X -= 1;
            }
            if (Input.IsActionPressed("move_down"))
            {
                direction.Y += 1;
            }
            if (Input.IsActionPressed("move_up"))
            {
                direction.Y -= 1;
            }
            entity.Direction = direction.Normalized();
        }
    }
}
