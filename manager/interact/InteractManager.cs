using Godot;
using MyGame.Entity;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class InteractManager : Node
	{
		private readonly List<Tuple<BaseInteractableDynamicEntity, BaseInteractableStaticEntity>> _interactablePairs = new();

		[Signal]
		public delegate void RegistrateInteractablePairEventHandler(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity);
		[Signal]
		public delegate void UnregistrateInteractablePairEventHandler(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity);

		private void AddPair(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
		{
			_interactablePairs.Add(Tuple.Create(dynamicEntity, staticEntity));
		}

        private void RemovePair(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
        {
            staticEntity.HideInteractionPrompt();

            var index = _interactablePairs.FindIndex(item => item.Item1 == dynamicEntity && item.Item2 == staticEntity);
            if (index != -1)
            {
                _interactablePairs.RemoveAt(index);
            }
        }

        public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("InteractManager", this);
			RegistrateInteractablePair += AddPair;
			UnregistrateInteractablePair += RemovePair;
		}

		public override void _ExitTree()
		{
			RegistrateInteractablePair -= AddPair;
			UnregistrateInteractablePair -= RemovePair;
			GlobalObjectManager.RemoveGlobalObject("InteractManager");
		}

		public override void _Process(double delta)
		{
			foreach (var pair in _interactablePairs)
			{
				pair.Item2.HideInteractionPrompt();
			}

			if(_interactablePairs.Count > 0)
			{
                _interactablePairs.Sort((a, b) =>
                {
                    return (a.Item1.Position - a.Item2.Position).Length().CompareTo((b.Item1.Position - b.Item2.Position).Length());
                });

				var closestPair = _interactablePairs[0];
				closestPair.Item2.ShowInteractionPrompt();

				if(Input.IsActionJustReleased("activate"))
				{
					closestPair.Item2.Interact(closestPair.Item1);
				}
            }
		}
	}
}
