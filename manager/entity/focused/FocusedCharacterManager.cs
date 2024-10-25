using Godot;
using MyGame.Entity;

namespace MyGame.Manager
{
	public partial class FocusedCharacterManager : Node
	{
		private BaseCharacter _focusedCharacter;

		public void FocusOnCharacter(BaseCharacter character)
		{
			_focusedCharacter = character;
		}

		public BaseCharacter GetFocusedCharacter()
		{
			return _focusedCharacter;
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("FocusedCharacterManager", this);
		}

		public override void _ExitTree()
		{
			GlobalObjectManager.RemoveGlobalObject("FocusedCharacterManager");
		}

		public override void _Process(double delta)
		{
			_focusedCharacter.InteractionManager.ShowNearestTip();
			if (Input.IsActionJustReleased("activate"))
			{
				_focusedCharacter.InteractionManager.Interact();
			}
		}
	}
}
