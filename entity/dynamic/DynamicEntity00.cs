using Godot;
using MyGame.Component;

namespace MyGame.Entity
{
	public partial class DynamicEntity00 : BaseInteractableDynamicEntity
	{
		public DynamicEntity00()
		{
			IsTransitable = true;
		}

		public override void _Ready()
		{
			LoadStrategy(() =>
            {
                return new InputNavigator();
            });
            LoadStrategy(() =>
            {
                return new CharacterAnimationPlayer(GetNode<AnimatedSprite2D>("AnimatedSprite2D"));
            });
			LoadStrategy(() =>
			{
				return new FrictionVelocityAlgorithm(this, 100, 2000, 1000);
			});
		}
	}
}
