using MyGame.Entity;

namespace MyGame.Component
{
    public interface IItemDropStrategy
    {
        public void DropItem(IEntity entity);
    }
}
