using Godot;
using MyGame.Manager;
using System;

namespace MyGame.Entity
{
	public abstract partial class BaseDynamicEntity: CharacterBody2D, IEntity
	{
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

		protected abstract void UpdateAnimation();

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

		private void UpdateTookOverDirection()
		{
			if ((_tookOverToPosition - Position).Length() < _maxVelocity / 30)
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

		private void UpdatePosition()
		{
			MoveAndSlide();
			//if (!Velocity.IsZeroApprox())
			//{
			//    GD.Print($"{_name}\ndirection: ({_direction.X}, {_direction.Y})\nvelocity: ({Velocity.X}, {Velocity.Y})\nposition: ({Position.X}, {Position.Y})");
			//}
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
			UpdateAnimation();
			UpdateVelocity(delta);
			UpdatePosition();
		}
	}
}
