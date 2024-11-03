using Godot;
using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public class TestItemPickupStrategy : IItemPickupStrategy
    {
        public void PickupItem(IEntity entity, BasicItem item)
        {
            GD.Print($"{entity.EntityName} pickup {item.ItemName}");
        }
    }
}
