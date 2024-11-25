using MyGame.Entity.MainBody;

namespace MyGame.Item.Strategy
{
    public class TestCheckItem : IItemOperation
    {
        public void Activate(BasicCharacter character, BasicItem item)
        {
            character.Say($"This item is {item.ItemName}.");
        }
    }
}
