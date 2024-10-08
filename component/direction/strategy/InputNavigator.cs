using Godot;

namespace MyGame.Component
{
    public class InputNavigator : INavigator
    {
        public Vector2 UpdateDirection()
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

            return direction.Normalized();
        }
    }
}
