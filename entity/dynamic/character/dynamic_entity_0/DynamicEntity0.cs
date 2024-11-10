using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BasicCharacter
	{
		public override HashSet<string> GetInteractionTags()
		{
			return new()
			{
				"Human"
			};
		}

		public override void InitializeStates(Dictionary<string, IState> states = null)
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

        public override void AfterInitialize()
        {
			FocusedCharacterManager.Instance.FocusedCharacter = this;
        }
    }
}
