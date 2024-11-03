﻿using MyGame.Entity;
using System.Collections.Generic;

namespace MyGame.Component
{
    public class CharacterSaveComponent : ISaveComponent
    {
        public List<string> ItemNameList { get; set; }

        public ISaveComponent Next { get; set; }

        public void SaveData(IEntity entity)
        {
            ItemNameList = ((BasicCharacter)entity).InventoryManager.GetItemNameList();
        }

        public void LoadData(IEntity entity)
        {
            ((BasicCharacter)entity).InitializeInventory(ItemNameList);
        }
    }
}
