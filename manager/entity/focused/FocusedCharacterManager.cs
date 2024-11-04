using Godot;
using MyGame.Entity;

namespace MyGame.Manager
{
	public partial class FocusedCharacterManager : Node
	{
		private BasicCharacter _focusedCharacter;

		public BasicCharacter FocusedCharacter
		{
			get => _focusedCharacter;
			set
			{
				_focusedCharacter = value;
				GlobalObjectManager.InitializeInventoryInterface(FocusedCharacter, 10);
			}
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
			if (FocusedCharacter != null)
			{
				FocusedCharacter.InteractionManager.ShowNearestTip();
				if (Input.IsActionJustReleased("activate"))
				{
					FocusedCharacter.InteractionManager.Interact();
				}
			}

			if (Input.IsActionJustReleased("save"))
			{
				GlobalObjectManager.Save("test.json");
			}

			if (Input.IsActionJustReleased("load"))
			{
				GlobalObjectManager.Load("test.json");
			}
		}
	}
}
