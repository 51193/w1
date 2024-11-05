﻿using Godot;
using MyGame.Component;
using MyGame.Interface;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public abstract partial class BasicCharacter : BasicInteractableDynamicEntity, IInteractionParticipant
    {
        public InteractionManager InteractionManager { get; init; }
        public InventoryManager InventoryManager { get; set; }

        [Export]
        public AnimatedSprite2D AnimationSprite2DNode;

        [Export]
        private SpeechBubble _speechBubble;

        public BasicCharacter()
        {
            InteractionManager = new(this);
        }

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

        public abstract HashSet<string> GetInteractionTags();

        protected override void WhenPositionChange()
        {
            base.WhenPositionChange();
            InteractionManager.ResortEntitiesOrder();
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
