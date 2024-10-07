using Godot;
using MyGame.Component;
using MyGame.Manager;
using System;

namespace MyGame.Entity
{
	public abstract partial class BaseDynamicEntity: CharacterBody2D, IEntity
	{
		protected LazyLoader<IAnimationPlayer> _animationPlayer;
		protected LazyLoader<IVelocityAlgorithm> _velocityAlgorithm;
        protected LazyLoader<INavigator> _navigator;

        public void LoadStrategy(Func<IAnimationPlayer> factory)
        {
            _animationPlayer = new LazyLoader<IAnimationPlayer>(factory);
        }
        public void LoadStrategy(Func<IVelocityAlgorithm> factory)
        {
            _velocityAlgorithm = new LazyLoader<IVelocityAlgorithm>(factory);
        }
        public void LoadStrategy(Func<INavigator> factory)
		{
			_navigator = new LazyLoader<INavigator>(factory);
		}

        private string _renderingOrderGroupName;
		public bool IsTransitable = false;

		private string _name;

		public Vector2 Direction = Vector2.Zero;
		private Vector2 _lastFramePosition = Vector2.Zero;

		public BaseDynamicEntity()
		{
			_name = GetType().Name;
		}

		public string GetEntityName() { return _name; }

		public string GetRenderingGroupName() {  return _renderingOrderGroupName; }

		public void SetRenderingGroupName(string groupName) {  _renderingOrderGroupName = groupName; }

		public virtual string GetState()
		{
			return null;
		}

		public virtual void SetState(string state) { }

		private void UpdateDirection()
		{
			if(_navigator == null) return;
			Direction = _navigator.Invoke(navigator => navigator.UpdateDirection());
		}

		private void UpdateVelocity(double delta)
		{
			if (_velocityAlgorithm == null) return;
			Velocity = _velocityAlgorithm.Invoke(algorithm => algorithm.UpdateVelocity(delta));
		}

		private void UpdateAnimation(double delta)
		{
			if (_animationPlayer == null) return;
			_animationPlayer.Invoke(player => player.UpdateAnimation(Direction, delta));
		}

		private void UpdateAnimation()
		{
			if( _animationPlayer == null) return;
			_animationPlayer.Invoke(player => player.UpdateAnimationWithoutConstraint(Direction));
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
			UpdateDirection();
			UpdateAnimation(delta);
			UpdateVelocity(delta);
			UpdatePosition();
		}

		public void EntityInitiateProcess()
		{
			UpdateDirection();
			UpdateAnimation();
			UpdateVelocity(0.001);
			UpdatePosition();
		}
	}
}
