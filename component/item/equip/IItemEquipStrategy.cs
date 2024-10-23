using MyGame.Entity;

namespace MyGame.Component
{
    public interface IItemEquipStrategy
    {
        public void EquipItem(IEntity entity);
    }
}
