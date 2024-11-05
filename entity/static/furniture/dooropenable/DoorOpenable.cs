using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DoorOpenable : BasicInteractableStaticEntity
	{
		[Export]
		public AnimationPlayer AnimationPlayerNode;

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
	}
}
