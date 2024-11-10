using Godot;
using MyGame.Entity;
using MyGame.Manager;
using MyGame.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.State
{
    public abstract class BasicState<T> : IState
    {
        private readonly List<IStrategy> _strategies = new();
        private readonly List<IStrategy> _processStrategies = new();
        private readonly List<IStrategy> _physicsProcessStrategies = new();

        public BasicState()
        {
            InitializeStrategies();
        }

        protected void AddStrategy<S>(StrategyGroup strategyGroup) where S : IStrategy, new()
        {
            IStrategy strategy = StrategyInstanceManager.Instance.GetInstance<S>();

            switch (strategyGroup)
            {
                case StrategyGroup.NormalStrategy:
                    _strategies.Add(strategy);
                    break;
                case StrategyGroup.ProcessStrategy:
                    _processStrategies.Add(strategy);
                    break;
                case StrategyGroup.PhysicsProcessStrategy:
                    _physicsProcessStrategies.Add(strategy);
                    break;
            }
        }

        protected abstract void InitializeStrategies();

        private void LoadStrategies(IEntity entity)
        {
            foreach (var strategy in _strategies)
            {
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.NormalStrategy);
            }

            foreach (var strategy in _processStrategies)
            {
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.ProcessStrategy);
            }

            foreach(var strategy in _physicsProcessStrategies)
            {
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.PhysicsProcessStrategy);
            }
        }

        private void UnloadStrategies(IEntity entity)
        {
            foreach (var strategy in _strategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.NormalStrategy);
            }

            foreach (var strategy in _processStrategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.ProcessStrategy);
            }

            foreach (var strategy in _physicsProcessStrategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.PhysicsProcessStrategy);
            }
        }

        public void Enter(IEntity entity)
        {
            if (entity is T typedEntity)
            {
                LoadStrategies(entity);
                Enter(typedEntity);
                GD.Print($"{entity.EntityName} enter state: {GetType().Name}");
            }
            else
            {
                GD.PrintErr($"{entity.EntityName} can't fit in type: {typeof(T).FullName} when state enter");
            }
        }

        public abstract void Enter(T entity);

        public void Exit(IEntity entity)
        {
            if (entity is T typedEntity)
            {
                Exit(typedEntity);
                UnloadStrategies(entity);
                GD.Print($"{entity.EntityName} exit state: {GetType().Name}");
            }
            else
            {
                GD.PrintErr($"{entity.EntityName} can't fit in type: {typeof(T).FullName} when state enter");
            }
        }

        public abstract void Exit(T entity);

        public Type Transit(IEntity entity, string token, params object[] parameters)
        {
            if (entity is T typedEntity)
            {
                GD.Print($"{entity.EntityName} transit state: {GetType().Name}");
                return Transit(typedEntity, token, parameters);
            }
            else
            {
                GD.PrintErr($"{entity.EntityName} can't fit in type: {typeof(T).FullName} when state transit");
                return null;
            }
        }

        public abstract Type Transit(T entity, string token, params object[] parameters);
    }
}
