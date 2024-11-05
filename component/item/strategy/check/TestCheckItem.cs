using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public class TestCheckItem : IItemOperation
    {
        public void Activate(BasicCharacter character, BasicItem item)
        {
            character.Say($"This item is {item.ItemName}.");
        }
    }
}
