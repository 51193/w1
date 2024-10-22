using Godot;
using MyGame.Component;
using System;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BaseCharacter
	{
		private class HardwareInputControlState : IState
		{
			public void OnEnter(IEntity entity)
			{
				((DynamicEntity0)entity).LoadStrategy(() =>
				{
					return new InputNavigator();
				});
			}
			public void OnExit(IEntity entity) { }
			public void OnHandleStateTransition(IEntity entity, string input, params object[] args)
			{
				switch (input)
				{
					case "GoStraight":
						if (args.Length > 0 && args[0] is Vector2 position)
						{
							((DynamicEntity0)entity).EventManager.RegistrateEvent("OnReachedTarget",
								new Action(() =>
								{
									((DynamicEntity0)entity).StateManager.ChangeState("ControlState", new HardwareInputControlState());
								}));
							((DynamicEntity0)entity).StateManager.ChangeState("ControlState", new StraightForwardControlState(position, "OnReachedTarget"));
						}
						break;
				}
			}
		}
		private class StraightForwardControlState : IState
		{
			private readonly Vector2 _targetPosition;
			private readonly string _callbackName;

			private uint _entityOriginalCollisionMask = 0;
			private bool _entityOriginalTransitableState = false;
			public StraightForwardControlState(Vector2 targetPosition, string callbackName)
			{
				_targetPosition = targetPosition;
				_callbackName = callbackName;
			}
			public void OnEnter(IEntity entity)
			{
				_entityOriginalCollisionMask = ((DynamicEntity0)entity).CollisionMask;
				_entityOriginalTransitableState = ((DynamicEntity0)entity).IsTransitable;
				((DynamicEntity0)entity).CollisionMask = 0;
				((DynamicEntity0)entity).IsTransitable = false;
				((DynamicEntity0)entity).LoadStrategy(() =>
				{
					return new StraightForwardToTargetNavigator((DynamicEntity0)entity, _targetPosition, () =>
					{
						((DynamicEntity0)entity).EventManager.TriggerEvent(_callbackName);
					});
				});
			}
			public void OnExit(IEntity entity)
			{
				((DynamicEntity0)entity).CollisionMask = _entityOriginalCollisionMask;
				((DynamicEntity0)entity).IsTransitable = _entityOriginalTransitableState;
				((DynamicEntity0)entity).EventManager.UnregistrateEvent(_callbackName);
			}
			public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
		}
		private class NormalState : IState
		{
			public void OnEnter(IEntity entity)
			{
				((DynamicEntity0)entity).LoadStrategy(() =>
				{
					return new CharacterAnimationPlayer(((DynamicEntity0)entity)._animationSprite2DNode);
				});
				((DynamicEntity0)entity).LoadStrategy(() =>
				{
					return new FrictionVelocityAlgorithm(((DynamicEntity0)entity), 100, 2000, 1000);
				});
			}
			public void OnExit(IEntity entity) { }
			public void OnHandleStateTransition(IEntity entity, string input, params object[] args) { }
		}
	}
}
