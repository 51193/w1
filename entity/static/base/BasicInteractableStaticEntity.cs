using Godot;
using MyGame.Component;
using System;

namespace MyGame.Entity
{
    public abstract partial class BasicInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        [Export]
        public Label Tip;

        public LazyLoader<IInteractionStrategy> InteractionStrategy { get; set; }

        public void LoadStrategy(Func<IInteractionStrategy> factory)
        {
            InteractionStrategy = new LazyLoader<IInteractionStrategy>(factory);
        }

        public virtual void ShowTip()
        {
            Tip.Show();
        }

        public virtual void HideTip()
        {
            Tip.Hide();
        }
    }
}
