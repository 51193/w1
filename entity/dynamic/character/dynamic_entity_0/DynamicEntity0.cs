using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BaseCharacter
	{
		public AnimatedSprite2D AnimationSprite2DNode;

		public DynamicEntity0()
		{
			GlobalObjectManager.FocusOnCharacter(this);
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
				StateManager = new(this, new()
				{
					["OverallState"] = new CharacterDefaultState(),
					["ControlState"] = new CharacterHardwareInputControlState()
				});
			}
			else
			{
				StateManager = new(this, states);
			}
		}

		public override void ShowTip()
		{
			//Do nothing
		}

		public override void HideTip()
		{
			//Do nothing
		}

		public override void _Ready()
		{
			AnimationSprite2DNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}
    }
}
