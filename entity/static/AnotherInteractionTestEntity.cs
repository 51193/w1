using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class AnotherInteractionTestEntity : BaseInteractableStaticEntity
	{
		public override void _Ready()
		{
			InitInteractionPrompt(GetNode<Label>("Label"));
			LoadStrategy(() =>
			{
				return new TestStrategy();
			});
		}
	}
}
