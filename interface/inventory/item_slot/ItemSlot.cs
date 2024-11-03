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
			//Scale and Position is incorrect now
			Item = item;

			float scaleFactor = Mathf.Min(Size.X / Item.Size.X, Size.Y / Item.Size.Y);
			Item.Scale = new Vector2(scaleFactor, scaleFactor);

			Vector2 scaledSize = scaleFactor * Item.Size;
			Vector2 offset = (Size - scaledSize) / 2;

			Item.Position = Position + offset;

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
