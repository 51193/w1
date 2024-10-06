using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class DynamicEntity00 : BaseInteractableDynamicEntity
	{
		public DynamicEntity00()
		{
			IsTransitable = true;
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
				return new CharacterAnimationPlayer(GetNode<AnimatedSprite2D>("AnimatedSprite2D"));
			});
			_velocityAlgorithm = new LazyLoader<IVelocityAlgorithm>(() =>
			{
				return new FrictionVelocityAlgorithm(100, 2000, 1000);
			});
		}
	}
}
