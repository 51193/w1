using Godot;

namespace MyGame.Entity
{
	public partial class Entity01 : BaseEntity
	{
		private AnimatedSprite2D _animatedSprite2D;
		private CollisionShape2D _collisionShape2D;

		public Entity01()
		{
			IsTransitable = false;

			_name = "Entity01";

			_maxVelocity = 400;
			_accelertion = 8000;
			_friction = 4000;

			SetTookOverPosition(100, new Vector2(-100, -100));
		}

		private void PlayAnimation(string animationName)
		{
			switch (animationName)
			{
				case "idle":
					{
						_animatedSprite2D.Offset = Vector2.Zero;
						_animatedSprite2D.Play("idle");
						break;
					}
				case "run":
					{
						_animatedSprite2D.Offset = new Vector2(0, -16);
						_animatedSprite2D.Play("run");
						break;
					}
				case "death":
					{
						_animatedSprite2D.Offset = Vector2.Zero;
						_animatedSprite2D.Play("death");
						break;
					}
			}
		}

		protected override void UpdateDirection()
		{
			_direction = Vector2.Zero;
		}

		protected override void UpdateAnimation()
		{
			if (_direction.IsZeroApprox())
			{
				if (_animatedSprite2D.Animation != "idle")
				{
					PlayAnimation("idle");
				}
				return;
			}

			if (_animatedSprite2D.Animation != "run")
			{
				PlayAnimation("run");
			}
			if (_direction.X < 0)
			{
				_animatedSprite2D.FlipH = true;
				return;
			}
			if (_direction.X > 0)
			{
				_animatedSprite2D.FlipH = false;
				return;
			}
		}

		public override void _EnterTree()
		{
			base._EnterTree();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
		}

		public override void _Ready()
		{
			_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		}

		public override void _Process(double delta)
		{
		}
	}
}
