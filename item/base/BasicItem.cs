using Godot;
using MyGame.Component;
using MyGame.Entity.MainBody;
using MyGame.Item.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.Item
{
	public abstract partial class BasicItem : Node2D
	{
		private readonly Dictionary<string, LazyLoader<IItemOperation>> _itemStrategeies = new();
		[Export]
		public string ItemPopupMenuName = "DefaultItemPopupMenu";
		[Export]
		private AnimationPlayer _iconAnimationPlayer;
		
		public string ItemName;
		public abstract Vector2 Size { get; }

		public BasicItem()
		{
			ItemName = GetType().Name;
			GD.Print($"{ItemName} initialized");
		}

		protected void AddItemStrategy(string key, Func<IItemOperation> factory)
		{
			if (_itemStrategeies.ContainsKey(key))
			{
				GD.PrintErr($"Duplicate item strategy name '{key}' in '{ItemName}'");
				return;
			}

			_itemStrategeies[key] = new LazyLoader<IItemOperation>(factory);
		}

		public void ActivateItemStrategy(BasicCharacter character, string key)
		{
			if (!_itemStrategeies.ContainsKey(key))
			{
				GD.PrintErr($"Invalid item strategy name '{key}' in '{ItemName}'");
			}

			_itemStrategeies[key].Invoke(strategy => strategy.Activate(character, this));
		}

		public abstract void InitializeStrategy();
		public abstract void InitializeAnimation();

		public void ChangeIconAnimation(string animationName)
		{
			_iconAnimationPlayer.Play(animationName);
		}

		public AnimationPlayer GetIconAnimationPlayer()
		{
			return _iconAnimationPlayer;
		}

		public override void _Ready()
		{
			InitializeStrategy();
			InitializeAnimation();
		}
	}
}
