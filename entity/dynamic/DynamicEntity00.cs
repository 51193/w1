using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class DynamicEntity00 : BaseDynamicEntity
	{
		public DynamicEntity00()
		{
			IsTransitable = true;
			_name = "DynamicEntity00";
		}

		protected override void UpdateDirection()
		{
			_direction = Vector2.Zero;

			if (Input.IsActionPressed("move_right"))
			{
				_direction.X += 1;
			}

			if (Input.IsActionPressed("move_left"))
			{
				_direction.X -= 1;
			}

			if (Input.IsActionPressed("move_down"))
			{
				_direction.Y += 1;
			}

			if (Input.IsActionPressed("move_up"))
			{
				_direction.Y -= 1;
			}

			_direction = _direction.Normalized();
		}

		public override void _Ready()
		{
			_animationPlayer = new LazyLoader<IAnimationPlayer>(() =>
			{
				var animationPlayer = new CharacterAnimationPlayer(GetNode<AnimatedSprite2D>("AnimatedSprite2D"));
				return animationPlayer;
			});
		}
	}
}
