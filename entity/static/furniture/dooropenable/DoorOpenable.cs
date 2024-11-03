using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BasicInteractableStaticEntity
	{
		public AnimationPlayer AnimationPlayerNode;
		public Label LabelNode;

		public override void ShowTip()
		{
			LabelNode.Show();
		}

		public override void HideTip()
		{
			LabelNode.Hide();
		}

		public override void InitializeStates(Dictionary<string, IState> states = null)
		{
			if (states == null || states.Count == 0)
			{
				StateManager = new(this, new Dictionary<string, IState>()
				{
					["OpenState"] = new DoorOpenableDoorClosedState(),
					["StrategyState"] = new DoorOpenableDefaultStrategyState()
				});
			}
			else
			{
				if (states["OpenState"] is DoorOpenableDoorClosingState)
				{
					states["OpenState"] = new DoorOpenableDoorClosedState();
				}
				else if (states["OpenState"] is DoorOpenableDoorOpeningState)
				{
					states["OpenState"] = new DoorOpenableDoorOpenedState();
				}
				StateManager = new(this, states);
			}
		}

		public override void _Ready()
		{
			AnimationPlayerNode = GetNode<AnimationPlayer>("AnimationPlayer");
			LabelNode = GetNode<Label>("Label");
			LabelNode.Hide();
		}
	}
}
