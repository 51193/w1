using Godot;
using MyGame.Component;
using MyGame.Manager;
using System;

namespace MyGame.Entity
{
	public abstract partial class BaseDynamicEntity: CharacterBody2D, IEntity
	{
		protected LazyLoader<IAnimationPlayer> _animationPlayer;

		private string _renderingOrderGroupName;
		public bool IsTransitable = false;

		protected string _name = "BaseDynamicEntity(shouldn't display)";

		protected float  _maxVelocity = 400;
		protected float _accelertion = 8000;
		protected float _friction = 4000;

		protected Vector2 _direction = Vector2.Zero;

		private bool _isTookOver = false;
		private float _tookOverMaxVelocity = 0;
		private Vector2 _tookOverToPosition = Vector2.Zero;

        protected abstract void UpdateDirection();

		public string GetEntityName() { return _name; }

		public string GetRenderingGroupName() {  return _renderingOrderGroupName; }

		public void SetRenderingGroupName(string groupName) {  _renderingOrderGroupName = groupName; }

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
			if(_accelertion < _friction)
			{
				GD.Print($"{_name}'s acceleration is smaller than friction");
				return;
			}

			if (Velocity.Length() < _friction * (float)delta)
			{
				Velocity = Vector2.Zero;
			}
			else
			{
				Velocity -= Velocity.Normalized() * _friction * (float)delta;
			}

			if (!_direction.IsNormalized())
			{
				_direction.Normalized();
			}

			Velocity += _direction * _accelertion * (float)delta;

			float maxVelocity = _maxVelocity;
			if(_isTookOver)
			{
				maxVelocity = _tookOverMaxVelocity;
			}

			Velocity *= (Math.Min(1, maxVelocity / Velocity.Length()));

			_direction = Vector2.Zero;

			if (!Velocity.IsZeroApprox() && _renderingOrderGroupName != null)
			{
				GlobalObjectManager.EmitResortRenderingOrderSignal(_renderingOrderGroupName);
			}
		}

		private void UpdateAnimation(double delta)
		{
			if (_animationPlayer == null) return;
			_animationPlayer.Invoke(player => player.UpdateAnimation(_direction, delta));
		}

        private void UpdatePosition()
		{
			MoveAndSlide();
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