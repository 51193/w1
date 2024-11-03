﻿using Godot;
using MyGame.Entity;
using MyGame.Item;

namespace MyGame.Component
{
    internal class TestItemUseStrategy : IItemUseStrategy
    {
        public void UseItem(IEntity entity, BasicItem item)
        {
            GD.Print($"{entity.EntityName} use {item.ItemName}");
        }
    }
}
