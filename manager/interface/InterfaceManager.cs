using Godot;
using MyGame.Entity.MainBody;
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

        private static InterfaceManager _instance;
        public static InterfaceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GD.PrintErr("InterfaceManager is not available");
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
                GD.PrintErr("Duplicate InterfaceManager entered the tree, this is not allowed");
            }
        }

        public override void _ExitTree()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
