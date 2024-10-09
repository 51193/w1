﻿using MyGame.Entity;

namespace MyGame.Component
{
    public class BinaryStateInteractionStrategy : IInteractionStrategy
    {
        private readonly BaseStaticEntity _entity;
        private readonly string _name;

        public BinaryStateInteractionStrategy(BaseStaticEntity entity, string binaryStateName)
        {
            _entity = entity;
            _name = binaryStateName;
        }

        public void Interaction(BaseInteractableDynamicEntity dynamicEntity)
        {
            _entity.HandleStateTransition(_name, null);
        }
    }
}