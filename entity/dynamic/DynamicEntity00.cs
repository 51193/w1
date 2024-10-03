using Godot;

namespace MyGame.Entity
{
	public partial class DynamicEntity00 : BaseDynamicEntity
	{
		private AnimatedSprite2D _animatedSprite2D;
		private CollisionShape2D _collisionShape2D;
		private Camera2D _camera2D;

		public DynamicEntity00()
		{
			IsTransitable = true;

			_name = "DynamicEntity00";

			_maxVelocity = 400;
			_accelertion = 8000;
			_friction = 4000;
		}

		private void UpdateInputEvent()
		{
			//TODO: this is a test function=
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

		private double animationChangeCooldownTimer = 1;
		protected override void UpdateAnimation(double delta)
		{
			animationChangeCooldownTimer += delta;
			if(animationChangeCooldownTimer < 0.1)
			{
				return;
			}
			else
			{
				animationChangeCooldownTimer = 0;
			}

			string currentAnimation = _animatedSprite2D.Animation;
			string directionSuffix = currentAnimation[^2..];

			if (_direction.IsZeroApprox())
			{ 
				if (currentAnimation.StartsWith("run") || currentAnimation.StartsWith("idle"))
				{
					string newIdleAnimation = "idle" + directionSuffix;

					if (_animatedSprite2D.Animation != newIdleAnimation)
					{
						_animatedSprite2D.Play(newIdleAnimation);
					}
				}
				return;
			}

			float angle = Mathf.Atan2(_direction.Y, _direction.X); 
			angle = Mathf.RadToDeg(angle); 

			if (angle >= -45 && angle < 45)
			{
				if (directionSuffix == "-4")
				{
					if (angle <= 0)
					{
						_animatedSprite2D.Play("run-2");
					}
					else
					{
						_animatedSprite2D.Play("run-8");
					}
				}
				else if (_animatedSprite2D.Animation != "run-6")
				{
					_animatedSprite2D.Play("run-6");
				}
			}
			else if (angle >= 45 && angle < 135)
			{
				if (directionSuffix == "-8")
				{
					if (angle <= 90)
					{
						_animatedSprite2D.Play("run-6");
					}
					else
					{
						_animatedSprite2D.Play("run-4");
					}
				}
				else if (_animatedSprite2D.Animation != "run-2")
				{
					_animatedSprite2D.Play("run-2");
				}
			}
			else if (angle >= 135 || angle < -135)
			{
				if (directionSuffix == "-6")
				{
					if (angle < 180)
					{
						_animatedSprite2D.Play("run-8");
					}
					else
					{
						_animatedSprite2D.Play("run-2");
					}
				}
				else if (_animatedSprite2D.Animation != "run-4")
				{
					_animatedSprite2D.Play("run-4");
				}
			}
			else if (angle >= -135 && angle < -45)
			{
				if (directionSuffix == "-2")
				{
					if (angle < -90)
					{
						_animatedSprite2D.Play("run-6");
					}
					else
					{
						_animatedSprite2D.Play("run-4");
					}
				}
				else if (_animatedSprite2D.Animation != "run-8")
				{
					_animatedSprite2D.Play("run-8");
				}
			}
		}


		public override void _Ready()
		{
			_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
			_camera2D = GetNode<Camera2D>("Camera2D");
		}

		public override void _Process(double delta)
		{
			UpdateInputEvent();
		}
	}
}
