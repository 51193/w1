using Godot;
using MyGame.Entity.Component;

namespace MyGame.Entity
{
    public abstract partial class BasicInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        [Export]
        public CanvasItem Tip { get; set; }
    }
}
