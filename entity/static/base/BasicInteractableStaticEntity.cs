using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
    public abstract partial class BasicInteractableStaticEntity : StaticEntity, IInteractableEntity
    {
        [Export]
        public Label Tip;

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
