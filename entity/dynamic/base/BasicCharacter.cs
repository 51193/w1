using Godot;
using MyGame.Component;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public abstract partial class BasicCharacter : BasicInteractableDynamicEntity, IInteractionParticipant
    {
        public InteractionManager InteractionManager { get; init; }
        public InventoryManager InventoryManager { get; set; }

        public AnimatedSprite2D AnimationSprite2DNode;

        public BasicCharacter()
        {
            InteractionManager = new(this);
        }

        public override ISaveComponent SaveData(ISaveComponent saveComponent = null)
        {
            return HandleSaveData<CharacterSaveComponent>(saveComponent);
        }

        public override ISaveComponent LoadData(ISaveComponent saveComponent)
        {
            return HandleLoadData<CharacterSaveComponent>(saveComponent);
        }

        public abstract HashSet<string> GetInteractionTags();

        protected override void WhenPositionChange()
        {
            base.WhenPositionChange();
            InteractionManager.ResortEntitiesOrder();
        }

        public void InitializeInventory(List<string> ItemNameList)
        {
            InventoryManager = new(this, ItemNameList);
        }

        public override void _Ready()
        {
            AnimationSprite2DNode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }
    }
}
