using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BaseInteractableStaticEntity
	{
		private AnimationPlayer _animationPlayerNode;
		private Label _label;

        public override void ShowTips()
        {
			_label.Show();
        }

        public override void HideTips()
        {
			_label.Hide();
        }

        public override void InitiateStates(Dictionary<string, IState> states = null)
        {
			if (states == null)
			{
				_stateManager = new(this, new Dictionary<string, IState>()
				{
					["OpenState"] = new DoorClosedState(),
					["StrategyState"] = new DefaultStrategeyState()
				});
			}
			else
			{
                if (states["OpenState"] is DoorClosingState)
				{
					states["OpenState"] = new DoorClosedState();
                }
				else if (states["OpenState"] is DoorOpeningState)
				{
					states["OpenState"] = new DoorOpenedState();
                }
                _stateManager = new(this, states);
            }
        }

        public override void _Ready()
		{
            _animationPlayerNode = GetNode<AnimationPlayer>("AnimationPlayer");
			_label = GetNode<Label>("Label");
			_label.Hide();
		}
	}
}
