using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    public interface IItemEquipStrategy
    {
        public void EquipItem(IEntity entity, BaseItem item);
    }
}
