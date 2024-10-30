using MyGame.Component;

namespace MyGame.Item
{
	public partial class TestItem0 : BaseItem
	{
		public override void InitiateAnimation()
		{
			ChangeIconAnimation("default");
		}

		public override void InitiateStrategy()
		{
			LoadStrategy(() => new TestItemDropStrategy());
			LoadStrategy(() => new TestItemEquipStrategy());
			LoadStrategy(() => new TestItemPickupStrategy());
			LoadStrategy(() => new TestItemUseStrategy());
		}
	}
}
