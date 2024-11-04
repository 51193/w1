using Godot;

namespace MyGame.Interface
{
	public partial class DefaultItemPopupMenu : BasicItemPopupMenu
	{
		protected override void InitializePopupItems()
		{
			AddItem("nothing", 0);
		}

		protected override void OnPopupMenuPressed(long id)
		{
			switch(id)
			{
				case 0:
					GD.Print("Nothing happened");
					break;
			}
		}
	}
}
