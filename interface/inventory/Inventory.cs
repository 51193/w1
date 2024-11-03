using Godot;
using MyGame.Item;
using System.Collections.Generic;

namespace MyGame.Interface
{
	public partial class Inventory : Control
	{
		private HBoxContainer _itemSlotContainer;
		private readonly List<ItemSlot> _itemSlots = new();
		public int VisibleSlotCount = 10;

		private void UpdateItemSlotLayout()
		{
			float screenWidth = GetViewportRect().Size.X;

			float slotWidth = Mathf.Min(screenWidth / VisibleSlotCount, 100);
			foreach (ItemSlot itemSlot in _itemSlots)
			{
				itemSlot.CustomMinimumSize = new Vector2(slotWidth, slotWidth);
			}
		}

		public void AttachItemToSlot(int slotIndex, BasicItem item)
		{
			if(slotIndex < VisibleSlotCount)
			{
				_itemSlots[slotIndex].Initialize(item);
			}
		}

		public override void _Ready()
		{
			_itemSlotContainer = GetNode<HBoxContainer>("HBoxContainer");

			for (int i = 0; i < VisibleSlotCount; i++)
			{
				ItemSlot itemSlot = new();
				_itemSlotContainer.AddChild(itemSlot);
				_itemSlots.Add(itemSlot);
			}

			UpdateItemSlotLayout();
		}
	}
}
