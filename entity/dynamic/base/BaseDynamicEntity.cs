using Godot;
using MyGame.Component;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public abstract partial class BaseDynamicEntity: CharacterBody2D, IEntity
	{
		public StateManager StateManager { get; set; }
        public EventManager EventManager { get; init; } = new();

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

        public string RenderingOrderGroupName { get; set; }
		public bool IsTransitable = true;

		public string EntityName { get; init; }

		public Vector2 Direction = Vector2.Zero;
		private Vector2 _lastFramePosition = Vector2.Zero;

        public BaseDynamicEntity()
		{
			EntityName = GetType().Name;
		}

		public virtual void InitiateStates(Dictionary<string, IState> states)
		{
            {
                if (states == null)
                {
                    StateManager = new(this);
                }
                else
                {
                    StateManager = new(this, states);
                }
            }
        }

		public virtual ISaveComponent SaveData(ISaveComponent saveComponent = null)
		{
			BaseSaveComponent save = new();
			save.SaveData(this);
			save.Next = saveComponent;
			return save;
		}

		public virtual ISaveComponent LoadData(ISaveComponent saveComponent)
		{
			saveComponent.LoadData(this);
			return saveComponent.Next;
		}

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
				WhenPositionChange();
                _lastFramePosition = Position;
            }
        }

		protected virtual void WhenPositionChange()
		{
            if (RenderingOrderGroupName != null)
            {
                GlobalObjectManager.ResortRenderingOrder(RenderingOrderGroupName);
            }
        }

		public override void _EnterTree()
		{
			GD.Print($"Dynamic entity enter: {EntityName}");
		}

		public override void _ExitTree()
		{
			GD.Print($"Dynamic entity exit: {EntityName}");
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
