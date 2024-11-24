using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.Manager;
using System;

namespace MyGame.Entity.Component
{
    public interface IInteractableEntity
    {
        public Vector2 Position { get; set; }
        public StrategyManager StrategyManager { get; set; }
        public DataManager DataManager { get; set; }
        public CanvasItem Tip { get; set; }

        //Just a syntax candy, cannot make sure InteractionStrategyTypeData and corresponding strategy is exist
        public void Interact()
        {
            Type type = DataManager.TryGet<InteractionStrategyTypeData>().InteractionStrategyType;
            if (type == null)
            {
                return;
            }
            StrategyManager.ActivateStrategy(type);
        }

        public void ShowTip()
        {
            Tip.Show();
        }

        public void HideTip()
        {
            Tip.Hide();
        }
    }
}
