using Godot;
using MyGame.Component;

namespace MyGame.Item
{
	public partial class TestItem0 : BasicItem
	{
		[Export]
		private AnimatedSprite2D _animatedSprite2DNode;

		public override string ItemPopupMenuName => "TestItemPopupMenu";

        public override Vector2 Size => _animatedSprite2DNode.SpriteFrames.GetFrameTexture("default", 0).GetSize();

		public override void InitializeAnimation()
		{
			ChangeIconAnimation("default");
		}

		public override void InitializeStrategy()
		{
			AddItemStrategy("check", () => { return new TestCheckItem(); });
		}
	}
}
