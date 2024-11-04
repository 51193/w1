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

			Vector2 actualSize = new(Mathf.Max(CustomMinimumSize.X, Size.X), Mathf.Max(CustomMinimumSize.Y, Size.Y));

            float scaleFactor = Mathf.Min(actualSize.X / Item.Size.X, actualSize.Y / Item.Size.Y);
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
