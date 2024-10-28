using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BaseCharacter
	{
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
			if (states == null || states.Count == 0)
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
    }
}
