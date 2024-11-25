using MyGame.Manager;

namespace MyGame.Entity.MainBody
{
	public partial class DynamicEntity0 : BasicPlayer
	{
		public override void AfterInitialize()
		{
			FocusedCharacterManager.Instance.FocusedCharacter = this;
		}
	}
}
