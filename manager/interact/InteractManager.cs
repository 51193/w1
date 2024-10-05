using Godot;
using MyGame.Entity;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class InteractManager : Node
	{
		private readonly List<KeyValuePair<BaseInteractableDynamicEntity, BaseInteractableStaticEntity>> _interactablePairs = new();

		[Signal]
		public delegate void RegistrateInteractablePairEventHandler(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity);
		[Signal]
		public delegate void UnregistrateInteractablePairEventHandler(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity);

		private void AddPair(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
		{
			_interactablePairs.Add(new KeyValuePair<BaseInteractableDynamicEntity, BaseInteractableStaticEntity>(dynamicEntity, staticEntity));
		}

		private void RemovePair(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
		{
			_interactablePairs.RemoveAll(item =>
			{
				return item.Key == dynamicEntity && item.Value == staticEntity;
			});
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
			_interactablePairs.Sort((a, b) =>
			{
				return (a.Key.Position - a.Value.Position).Length().CompareTo((b.Key.Position - b.Value.Position).Length());
			});
			if (_interactablePairs.Count > 0)
			{
				GD.Print(_interactablePairs[0].Value.GetEntityName());
			}
		}
	}
}
