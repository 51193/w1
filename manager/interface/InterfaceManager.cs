using Godot;
using MyGame.Entity;
using MyGame.Manager;

namespace MyGame.Interface
{
	public partial class InterfaceManager : Node 
	{
		[Export]
		private CanvasLayer _canvasLayer;
		[Export]
		private Inventory _inventory;
		[Export]
		private ItemPopupMenuManager _itemPopupMenuManager;

		public void InitializeInventoryInterface(BasicCharacter character, int visibleSlotCount)
		{
			_inventory.Initialize(character, visibleSlotCount);
		}

		public BasicItemPopupMenu GetItemPopupMenu(string itemPopupMenuName)
		{
			return _itemPopupMenuManager.GetItemPopupMenu(itemPopupMenuName);
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("InterfaceManager", this);
		}

		public override void _ExitTree()
		{
			GlobalObjectManager.RemoveGlobalObject("InterfaceManager");
		}
	}
}
