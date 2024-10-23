using Godot;
using MyGame.Component;
using MyGame.Entity;
using System;

namespace MyGame.Item
{
    public partial class BaseItem : Node2D
    {
        private AnimationPlayer _iconAnimationPlayer;

        public LazyLoader<IItemUseStrategy> ItemUseStrategy;
        public LazyLoader<IItemEquipStrategy> ItemEquipStrategy;
        public LazyLoader<IItemDropStrategy> ItemDropStrategy;

        public void LoadStrategy(Func<IItemUseStrategy> factory)
        {
            ItemUseStrategy = new LazyLoader<IItemUseStrategy>(factory);
        }
        public void LoadStrategy(Func<IItemEquipStrategy> factory)
        {
            ItemEquipStrategy = new LazyLoader<IItemEquipStrategy>(factory);
        }
        public void LoadStrategy(Func<IItemDropStrategy> factory)
        {
            ItemDropStrategy = new LazyLoader<IItemDropStrategy>(factory);
        }

        public string ItemName;

        public BaseItem()
        {
            ItemName = GetType().Name;
        }

        public void ChangeIconAnimation(string animationName)
        {
            _iconAnimationPlayer.Play(animationName);
        }

        public void UseItem(IEntity entity)
        {
            ItemUseStrategy.Invoke(strategy => strategy.UseItem(entity));
        }

        public void EquipItem(IEntity entity)
        {
            ItemEquipStrategy.Invoke(strategy => strategy.EquipItem(entity));
        }

        public void DropItem(IEntity entity)
        {
            ItemDropStrategy.Invoke(strategy => strategy.DropItem(entity));
        }

        public override void _Ready()
        {
            _iconAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }
    }
}
