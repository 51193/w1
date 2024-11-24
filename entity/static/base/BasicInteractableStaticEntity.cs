using Godot;
using MyGame.Entity.Component;
using System;

namespace MyGame.Entity
{
    public abstract partial class BasicInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        [Export]
        public CanvasItem Tip { get ; set; }
    }
}
