using Godot;
using MyGame.Entity;
using MyGame.Interface;
using System;

namespace MyGame.Manager
{
	public partial class FocusedCharacterManager : Node
	{
		private BasicCharacter _focusedCharacter;
		private Inventory _inventory;

		public BasicCharacter FocusedCharacter
		{
			get => _focusedCharacter;
			set
			{
				_focusedCharacter = value;
				DisplayItem();
			}
		}

		private void DisplayItem()
		{
			if (_focusedCharacter == null) return;

			int itemCount = Math.Min(_focusedCharacter.InventoryManager.Items.Count, _inventory.VisibleSlotCount);

			for (int i = 0; i < itemCount; i++)
			{
				_inventory.AttachItemToSlot(i, _focusedCharacter.InventoryManager.Items[i]);
			}
		}

		public override void _Ready()
		{
			_inventory = GetNode<CanvasLayer>("CanvasLayer").GetNode<Inventory>("Inventory");
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
