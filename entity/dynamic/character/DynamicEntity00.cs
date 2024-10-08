using Godot;
using MyGame.Component;
using System.Collections.Generic;

namespace MyGame.Entity
{
	public partial class DynamicEntity00 : BaseInteractableDynamicEntity
	{
		public DynamicEntity00()
		{
			IsTransitable = true;
		}

        public override void InitiateStates(Dictionary<string, IState> states = null)
        {
			if(states == null)
			{
				_stateManager = new(this, new()
				{
					["OverallState"] = new NormalState()
				});
			}
			else
			{
				_stateManager = new(this, states);
			}
        }
    }
}
