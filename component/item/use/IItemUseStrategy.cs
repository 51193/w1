using MyGame.Entity;

namespace MyGame.Component
{
    public interface IItemUseStrategy
    {
        public void UseItem(IEntity entity);
    }
}
