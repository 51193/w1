using Godot;
using MyGame.Component;
using MyGame.Manager;

namespace MyGame.Entity
{
	public abstract partial class BaseDynamicEntity: CharacterBody2D, IEntity
	{
		protected LazyLoader<IAnimationPlayer> _animationPlayer;
		protected LazyLoader<IVelocityAlgorithm> _velocityAlgorithm;

		private string _renderingOrderGroupName;
		public bool IsTransitable = false;

		private string _name;

		protected Vector2 _direction = Vector2.Zero;
		private Vector2 _lastFramePosition = Vector2.Zero;

		private bool _isTookOver = false;
		private float _tookOverMaxVelocity = 0;
		private Vector2 _tookOverToPosition = Vector2.Zero;

		public BaseDynamicEntity()
		{
			_name = GetType().Name;
		}

        protected abstract void UpdateDirection();

		public string GetEntityName() { return _name; }

		public string GetRenderingGroupName() {  return _renderingOrderGroupName; }

		public void SetRenderingGroupName(string groupName) {  _renderingOrderGroupName = groupName; }

		public virtual string GetState()
		{
			return null;
		}

		public virtual void SetState(string state) { }

		public void SetTookOverPosition(float maxVelocity, Vector2 position)
		{
			_isTookOver = true;
			_tookOverMaxVelocity = maxVelocity;
			_tookOverToPosition = position;
		}

		public void DisableTookOver()
		{
			_isTookOver = false;
			_tookOverToPosition = Vector2.Zero;
		}

		public void PlayAnimation(string animationName)
		{
			if (_animationPlayer == null) return;
			_animationPlayer.Invoke(player => player.PlayAnimation(animationName));
		}

		private void UpdateTookOverDirection()
		{
			if ((_tookOverToPosition - Position).Length() < _tookOverMaxVelocity / 30)
			{
				_direction = Vector2.Zero;
			}
			else
			{
				_direction = (_tookOverToPosition - Position).Normalized();
			}
		}

		private void UpdateVelocity(double delta)
		{
			if (_velocityAlgorithm == null) return;
			Velocity = _velocityAlgorithm.Invoke(algorithm => algorithm.UpdateVelocity(Velocity, _direction, delta));
			_direction = Vector2.Zero;
		}

		private void UpdateAnimation(double delta)
		{
			if (_animationPlayer == null) return;
			_animationPlayer.Invoke(player => player.UpdateAnimation(_direction, delta));
		}

        private void UpdatePosition()
		{
			MoveAndSlide();
			if(_lastFramePosition != Position)
			{
				if (_renderingOrderGroupName != null)
				{
					GlobalObjectManager.EmitResortRenderingOrderSignal(_renderingOrderGroupName);
				}
				_lastFramePosition = Position;
            }
        }

		public override void _EnterTree()
		{
			GD.Print($"Dynamic entity enter: {_name}");
		}

		public override void _ExitTree()
		{
			GD.Print($"Dynamic entity exit: {_name}");
		}

        public override void _PhysicsProcess(double delta)
		{
			if (_isTookOver)
			{
				UpdateTookOverDirection();
			}
			else
			{
				UpdateDirection();
			}
			UpdateAnimation(delta);
			UpdateVelocity(delta);
			UpdatePosition();
		}
	}
}
