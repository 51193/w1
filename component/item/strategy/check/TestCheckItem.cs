using Godot;
using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public class TestCheckItem : IItemOperation
    {
        public void Activate(BasicCharacter character, BasicItem item)
        {
            GD.Print($"{character.EntityName} is checking {item.ItemName}");
        }
    }
}
