using Godot;
using MyGame.Entity.MainBody;
using MyGame.Item;

namespace MyGame.Interface
{
    public abstract partial class BasicItemPopupMenu : PopupMenu
    {
        protected BasicCharacter _character;
        protected BasicItem _item;

        public void Initialize(BasicCharacter character, BasicItem item, ItemSlot itemSlot)
        {
            _character = character;
            _item = item;

            Vector2I pos = (Vector2I)(itemSlot.Position + new Vector2(0, itemSlot.Size.Y));

            float scaleFactor = ((float)Size.Y) / ((float)Size.X);

            int width = (int)itemSlot.Size.X;
            int height = (int)(itemSlot.Size.X * scaleFactor);

            Popup(new Rect2I(pos, new Vector2I(width, height)));
        }

        protected abstract void InitializePopupItems();

        protected abstract void OnPopupMenuPressed(long id);

        public override void _Ready()
        {
            Clear();
            InitializePopupItems();
            IdPressed += OnPopupMenuPressed;
        }

        public override void _ExitTree()
        {
            IdPressed -= OnPopupMenuPressed;
        }
    }
}
