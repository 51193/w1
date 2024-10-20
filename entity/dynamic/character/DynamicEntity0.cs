using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BaseCharacter
	{
		private AnimatedSprite2D _animationSprite2DNode;

		public DynamicEntity0()
		{
			InteractionManager.SetDisplay(true);
			IsTransitable = true;
		}

		public override HashSet<string> GetInteractionTags()
		{
			return new()
			{
				"Human"
			};
		}

		public override void InitiateStates(Dictionary<string, IState> states = null)
		{
			if(states == null)
			{
				_stateManager = new(this, new()
				{
					["OverallState"] = new NormalState(),
					["ControlState"] = new HardwareInputControlState()
				});
			}
			else
			{
				_stateManager = new(this, states);
			}
		}

		public override void ShowTips()
		{
			//Do nothing
		}

		public override void HideTips()
		{
			//Do nothing
		}

		public override void _Ready()
		{
			_animationSprite2DNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}
    }
}
