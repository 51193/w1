using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public interface IItemPickupStrategy
    {
        public void PickupItem(IEntity entity, BasicItem item);
    }
}
