﻿using Godot;
using MyGame.Entity.Component;
using MyGame.Interface;
using System.Collections.Generic;

namespace MyGame.Entity.MainBody
{
    public partial class BasicPlayer : BasicCharacter, IInteractableEntityScanner
    {
        [Export]
        public Area2D ScanningArea { get; set; }
        public List<IInteractableEntity> AccessibleInteratableEntities { get; init; } = new();

        public override void _Ready()
        {
            this.InitializeInteractableScanner();
        }

        public override void AfterInitialize()
        {
            InterfaceManager.Instance.InitializeInventoryInterface(this, 10);
        }
    }
}
