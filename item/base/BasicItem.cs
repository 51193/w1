using Godot;
using MyGame.Component;
using MyGame.Entity;
using System;

namespace MyGame.Item
{
	public abstract partial class BasicItem : Node2D
	{
		private AnimationPlayer _iconAnimationPlayer;
		
		public LazyLoader<IItemUseStrategy> ItemUseStrategy;
		public LazyLoader<IItemEquipStrategy> ItemEquipStrategy;
		public LazyLoader<IItemPickupStrategy> ItemPickupStrategy;
		public LazyLoader<IItemDropStrategy> ItemDropStrategy;

		public void LoadStrategy(Func<IItemUseStrategy> factory)
		{
			ItemUseStrategy = new LazyLoader<IItemUseStrategy>(factory);
		}
		public void LoadStrategy(Func<IItemEquipStrategy> factory)
		{
			ItemEquipStrategy = new LazyLoader<IItemEquipStrategy>(factory);
		}
		public void LoadStrategy(Func<IItemPickupStrategy> factory)
		{
			ItemPickupStrategy = new LazyLoader<IItemPickupStrategy>(factory);
		}
		public void LoadStrategy(Func<IItemDropStrategy> factory)
		{
			ItemDropStrategy = new LazyLoader<IItemDropStrategy>(factory);
		}

		public string ItemName;
		public abstract Vector2 Size { get; }

		public BasicItem()
		{
			ItemName = GetType().Name;
			GD.Print($"{ItemName} initialized");
		}

		public abstract void InitializeStrategy();
		public abstract void InitializeAnimation();

		public void ChangeIconAnimation(string animationName)
		{
			_iconAnimationPlayer.Play(animationName);
		}

		public void UseItem(IEntity entity)
		{
			ItemUseStrategy.Invoke(strategy => strategy.UseItem(entity, this));
		}

		public void EquipItem(IEntity entity)
		{
			ItemEquipStrategy.Invoke(strategy => strategy.EquipItem(entity, this));
		}

		public void PickupItem(IEntity entity)
		{
			ItemPickupStrategy.Invoke(strategy => strategy.PickupItem(entity, this));
		}

		public void DropItem(IEntity entity)
		{
			ItemDropStrategy.Invoke(strategy => strategy.DropItem(entity, this));
		}

		public AnimationPlayer GetIconAnimationPlayer()
		{
			return _iconAnimationPlayer;
		}

		public override void _Ready()
		{
			_iconAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			InitializeStrategy();
			InitializeAnimation();
		}
	}
}
