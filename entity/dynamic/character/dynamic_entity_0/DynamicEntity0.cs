using MyGame.Component;
using MyGame.Manager;

namespace MyGame.Entity
{
	public partial class DynamicEntity0 : BasicPlayer
	{
		public override void AfterInitialize()
		{
			FocusedCharacterManager.Instance.FocusedCharacter = this;
		}
	}
}
