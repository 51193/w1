using Godot;
using MyGame.Entity;
using MyGame.Item;
using MyGame.Manager;

namespace MyGame.Interface
{
	public partial class ItemSlot : Button
	{
		private BasicCharacter _character;
		private BasicItem _item;

		private void ClearChildren()
		{
			foreach (var child in GetChildren())
			{
				RemoveChild(child);
			}
		}

		public void Initialize(BasicCharacter character, BasicItem item)
		{
			_character = character;

			ClearChildren();

			_item = item;

			Vector2 actualSize = new(Mathf.Max(CustomMinimumSize.X, Size.X), Mathf.Max(CustomMinimumSize.Y, Size.Y));

            float scaleFactor = Mathf.Min(actualSize.X / _item.Size.X, actualSize.Y / _item.Size.Y);
			_item.Scale = new Vector2(scaleFactor, scaleFactor);

			Vector2 offset = CustomMinimumSize / 2;

			_item.Position = offset;
            AddChild(_item);
        }

		public override void _Pressed()
		{
			if (_item != null)
			{
				GD.Print($"ItemSlot pressed, {_item.ItemName} inside the slot");
				BasicItemPopupMenu itemPopupMenu = InterfaceManager.Instance.GetItemPopupMenu(_item.ItemPopupMenuName);
				itemPopupMenu.Initialize(_character, _item, this);
			}
			else
			{
				GD.Print($"ItemSlot pressed, nothing inside ths slot");
			}
		}
	}
}
