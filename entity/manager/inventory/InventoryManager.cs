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

        private IEntity _entity;
        private readonly List<BaseItem> _items = new();

        public InventoryManager(IEntity entity, List<string> itemNameList = null)
        {
            _entity = entity;
            InitiateItems(itemNameList);
        }

        private void InitiateItems(List<string> itemNameList)
        {
            foreach (var itemName in itemNameList)
            {
                if (!_loadedItems.TryGetValue(itemName, out var item))
                {
                    item = GlobalObjectManager.GetResource(itemName);
                    _loadedItems[itemName] = item;
                }
                _items.Add(item.Instantiate<BaseItem>());
            }
        }

        public List<string> GetItemNameList()
        {
            return _items.Select(i => i.ItemName).ToList();
        }
    }
}
