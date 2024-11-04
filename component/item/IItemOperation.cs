using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public interface IItemOperation
    {
        public void Activate(BasicCharacter character, BasicItem item);
    }
}
