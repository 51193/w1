using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public class CharacterSaveComponent : ISaveComponent
    {
        public string TestText;

        private ISaveComponent _next;

        public ISaveComponent Next { get => _next; set => _next = value; }

        public void SaveData(IEntity entity)
        {
            TestText = entity.GetEntityName();
        }

        public void LoadData(IEntity entity)
        {
            GD.PrintErr(TestText);
        }
    }
}
