using Godot;
using MyGame.Entity;
using MyGame.Interface;

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
                InterfaceManager.Instance.InitializeInventoryInterface(FocusedCharacter, 10);
			}
		}

        private static FocusedCharacterManager _instance;
        public static FocusedCharacterManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GD.PrintErr("FocusedCharacterManager is not available");
                }
                return _instance;
            }
        }

        public override void _EnterTree()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                GD.PrintErr("Duplicate FocusedCharacterManager entered the tree, this is not allowed");
            }
        }

        public override void _ExitTree()
        {
            if (_instance == this)
            {
                _instance = null;
            }
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
                MapTransition.Instance.ToSaveData("test.json");
			}

			if (Input.IsActionJustReleased("load"))
			{
                MapTransition.Instance.FromSaveData("test.json");
			}
		}
	}
}
