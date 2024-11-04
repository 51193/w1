using Godot;
using MyGame.Item;
using MyGame.Manager;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Entity
{
    public class InventoryManager
    {
        private static readonly Dictionary<string, PackedScene> _loadedItems = new();

        public readonly List<BasicItem> Items = new();

        public InventoryManager(List<string> itemNameList = null)
        {
            InitializeItems(itemNameList);
        }

        private void InitializeItems(List<string> itemNameList)
        {
            foreach (var itemName in itemNameList)
            {
                if (!_loadedItems.TryGetValue(itemName, out var item))
                {
                    item = GlobalObjectManager.GetResource(itemName);
                    _loadedItems[itemName] = item;
                }
                Items.Add(item.Instantiate<BasicItem>());
            }
        }

        public List<string> GetItemNameList()
        {
            return Items.Select(i => i.ItemName).ToList();
        }

        public void DeleteAllItems()
        {
            foreach(var item in Items)
            {
                item.QueueFree();
            }
            Items.Clear();
        }
    }
}
