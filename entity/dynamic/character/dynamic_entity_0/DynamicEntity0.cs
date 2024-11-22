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
			//base.AfterInitialize();
			FocusedCharacterManager.Instance.FocusedCharacter = this;
		}
	}
}
