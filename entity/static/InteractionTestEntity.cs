using MyGame.Component;

namespace MyGame.Entity
{
	public partial class InteractionTestEntity : BaseInteractableStaticEntity
	{
		public InteractionTestEntity()
		{
			_name = "InteractionTestEntity";
		}

		public override void _Ready()
		{
			_interactionStrategy = new LazyLoader<IInteractionStrategy>(() =>
			{
				return new TestStrategy();
			});
		}
	}
}
