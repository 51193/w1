using Godot;
using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Interface
{
	public partial class Inventory : Control
	{
		private BasicCharacter _character;

		private HBoxContainer _itemSlotContainer;
		private readonly List<ItemSlot> _itemSlots = new();
		public int VisibleSlotCount;

		public void Initialize(BasicCharacter focusedCharacter, int visibleSlotCount)
		{
			_character = focusedCharacter;
			VisibleSlotCount = visibleSlotCount;

			RecomposeSlots();
		}

		private void UpdateItemSlotLayout()
		{
			float screenWidth = GetViewportRect().Size.X;

			float slotWidth = Mathf.Min(screenWidth / VisibleSlotCount, 100);
			foreach (ItemSlot itemSlot in _itemSlots)
			{
				itemSlot.CustomMinimumSize = new Vector2(slotWidth, slotWidth);
			}
		}

		private void ClearChildren()
		{
			foreach(ItemSlot itemSlot in _itemSlots)
			{
				itemSlot.QueueFree();
			}
			_itemSlots.Clear();
		}

		private void RecomposeSlots()
		{
			ClearChildren();

            for (int i = 0; i < VisibleSlotCount; i++)
            {
                ItemSlot itemSlot = new();
                _itemSlotContainer.AddChild(itemSlot);
                _itemSlots.Add(itemSlot);
            }

            UpdateItemSlotLayout();

            DisplayCharacterInventory();
        }

		private void DisplayCharacterInventory()
		{
			if (_character == null) return;

			int itemCount = Math.Min(_character.InventoryManager.Items.Count, VisibleSlotCount);

			for (int i = 0; i < itemCount; i++)
            {
				_itemSlots[i].Initialize(_character, _character.InventoryManager.Items[i]);
            }
		}

		public override void _Ready()
		{
			_itemSlotContainer = GetNode<HBoxContainer>("HBoxContainer");
		}
	}
}
