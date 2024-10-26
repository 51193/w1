using Godot;
using MyGame.Entity;

namespace MyGame.Component
{
    public class CharacterSaveComponent : ISaveComponent
    {
        public string TestText;

        public ISaveComponent Next { get; set; }

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
