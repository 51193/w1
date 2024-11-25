using MyGame.Entity.MainBody;

namespace MyGame.Item.Strategy
{
    public interface IItemOperation
    {
        public void Activate(BasicCharacter character, BasicItem item);
    }
}
