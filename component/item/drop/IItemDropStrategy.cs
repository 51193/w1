using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public interface IItemDropStrategy
    {
        public void DropItem(IEntity entity, BaseItem item);
    }
}
