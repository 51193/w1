using Godot;
using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public class TestItemEquipStrategy : IItemEquipStrategy
    {
        public void EquipItem(IEntity entity, BasicItem item)
        {
            GD.Print($"{entity.EntityName} equip {item.ItemName}");
        }
    }
}
