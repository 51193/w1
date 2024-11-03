using Godot;
using MyGame.Component;

namespace MyGame.Item
{
	public partial class TestItem0 : BasicItem
	{
		[Export]
		private AnimatedSprite2D _animatedSprite2DNode;

		public override Vector2 Size => _animatedSprite2DNode.SpriteFrames.GetFrameTexture("default", 0).GetSize();

		public override void InitializeAnimation()
		{
			ChangeIconAnimation("default");
		}

		public override void InitializeStrategy()
		{
			LoadStrategy(() => new TestItemDropStrategy());
			LoadStrategy(() => new TestItemEquipStrategy());
			LoadStrategy(() => new TestItemPickupStrategy());
			LoadStrategy(() => new TestItemUseStrategy());
		}
	}
}
