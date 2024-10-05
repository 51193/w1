using MyGame.Component;

namespace MyGame.Entity
{
	public partial class AnotherInteractionTestEntity : BaseInteractableStaticEntity
	{
		public AnotherInteractionTestEntity()
		{
			_name = "AnotherInteractionTestEntity";
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
