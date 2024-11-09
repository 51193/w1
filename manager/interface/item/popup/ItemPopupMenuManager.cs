using Godot;
using MyGame.Interface;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class ItemPopupMenuManager : Node
	{
		private readonly Dictionary<string, BasicItemPopupMenu> _loadedPopupMenu = new();

		private void LoadItemPopupMenu(string itemPopupMenuName)
		{
			if (_loadedPopupMenu.ContainsKey(itemPopupMenuName))
			{
				GD.PrintErr($"Duplicate popup menu loaded: {itemPopupMenuName}");
				return;
			}

			BasicItemPopupMenu popupMenu = ResourceManager.Instance.GetResource(itemPopupMenuName).Instantiate<BasicItemPopupMenu>();
			popupMenu.Hide();
            AddChild(popupMenu);
            _loadedPopupMenu[itemPopupMenuName] = popupMenu;
        }

		public BasicItemPopupMenu GetItemPopupMenu(string itemPopupMenuName)
		{
			if (!_loadedPopupMenu.ContainsKey(itemPopupMenuName))
			{
				LoadItemPopupMenu(itemPopupMenuName);
			}
			return _loadedPopupMenu[itemPopupMenuName];
		}
    }
}
