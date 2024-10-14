using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class InteractionManager : Node
	{
		private readonly List<Tuple<IInteractionParticipant, IInteractableEntity>> _interactablePairs = new();

		public void AddPair(IInteractionParticipant participant, IInteractableEntity interactableEntity)
		{
			_interactablePairs.Add(Tuple.Create(participant, interactableEntity));
		}

		public void RemovePair(IInteractionParticipant participant, IInteractableEntity interactableEntity)
		{
			var index = _interactablePairs.FindIndex(item => item.Item1 == participant && item.Item2 == interactableEntity);
			_interactablePairs[index].Item2.WhenParticipantIsNotNearest();
			if (index != -1)
			{
				_interactablePairs.RemoveAt(index);
			}
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("InteractManager", this);
		}

		public override void _ExitTree()
		{
			GlobalObjectManager.RemoveGlobalObject("InteractManager");
		}

		public override void _Process(double delta)
		{
			foreach (var pair in _interactablePairs)
			{
				pair.Item2.WhenParticipantIsNotNearest();
			}

			if(_interactablePairs.Count > 0)
			{
				_interactablePairs.Sort((a, b) =>
				{
					return (a.Item1.Position - a.Item2.Position).Length().CompareTo((b.Item1.Position - b.Item2.Position).Length());
				});

				var closestPair = _interactablePairs[0];
				closestPair.Item2.WhenParticipantIsNearest();
				if(Input.IsActionJustReleased("activate"))
				{
					if (closestPair.Item2.CanInteractWith(closestPair.Item1))
					{
						closestPair.Item2.Interact(closestPair.Item1);
					}
					else
					{
						GD.PrintErr("Activate an invalid interaction pair");
					}
				}
			}
		}
	}
}
