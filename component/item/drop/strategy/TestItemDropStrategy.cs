using Godot;
using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public class TestItemDropStrategy : IItemDropStrategy
    {
        public void DropItem(IEntity entity, BaseItem item)
        {
            GD.Print($"{entity.EntityName} drop {item.ItemName}");
        }
    }
}
