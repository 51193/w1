using Godot;
using MyGame.Entity.Manager;
using MyGame.Entity.Save;
using MyGame.Interface;
using System.Collections.Generic;

namespace MyGame.Entity.MainBody
{
    public abstract partial class BasicCharacter : DynamicEntity
    {
        public InventoryManager InventoryManager { get; set; }

        [Export]
        public AnimationPlayer AnimationPlayerNode;

        [Export]
        private SpeechBubble _speechBubble;

        public void Say(string text, float duration = 2)
        {
            _speechBubble.ShowSpeech(text, duration);
        }

        public override ISaveComponent SaveData(ISaveComponent saveComponent = null)
        {
            return HandleSaveData<CharacterSaveComponent>(saveComponent);
        }

        public override ISaveComponent LoadData(ISaveComponent saveComponent)
        {
            return HandleLoadData<CharacterSaveComponent>(saveComponent);
        }

        public void InitializeInventory(List<string> ItemNameList)
        {
            InventoryManager = new(ItemNameList);
        }

        public override void _ExitTree()
        {
            InventoryManager.DeleteAllItems();
            base._ExitTree();
        }
    }
}
