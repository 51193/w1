namespace MyGame.Interface
{
    public partial class TestItemPopupMenu : BasicItemPopupMenu
    {
        protected override void InitializePopupItems()
        {
            AddItem("Check", 0);
        }

        protected override void OnPopupMenuPressed(long id)
        {
            switch (id)
            {
                case 0:
                    _item.ActivateItemStrategy(_character, "check");
                    break;
            }
        }
    }
}
