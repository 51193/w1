using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public interface IItemUseStrategy
    {
        public void UseItem(IEntity entity, BasicItem item);
    }
}
