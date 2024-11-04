using Godot;
using MyGame.Item;

namespace MyGame.Interface
{
	public partial class ItemSlot : Button
	{
		public BasicItem Item;

		private void ClearAllChildren()
		{
			foreach (var child in GetChildren())
			{
				RemoveChild(child);
			}
		}

		public void Initialize(BasicItem item)
		{
			ClearAllChildren();

			Item = item;

            float scaleFactor = Mathf.Min(CustomMinimumSize.X / Item.Size.X, CustomMinimumSize.Y / Item.Size.Y);
			Item.Scale = new Vector2(scaleFactor, scaleFactor);

			Vector2 offset = CustomMinimumSize / 2;

			Item.Position = offset;
            AddChild(Item);
        }

		public override void _Pressed()
		{
			if (Item != null)
			{
				GD.Print($"ItemSlot pressed, {Item.ItemName} inside the slot");
			}
			else
			{
				GD.Print($"ItemSlot pressed, nothing inside ths slot");
			}
		}
	}
}
