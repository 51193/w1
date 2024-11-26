using Godot;
using MyGame.Entity.Data;
using MyGame.Entity.MainBody;
using MyGame.Entity.Manager;
using MyGame.Entity.Strategy;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.State
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

        public void LoadStrategies(IEntity entity)
        {
            foreach (var strategy in _strategies)
            {
                entity.DataManager.LoadStrategyData(strategy);
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.NormalStrategy);
            }

            foreach (var strategy in _processStrategies)
            {
                entity.DataManager.LoadStrategyData(strategy);
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.ProcessStrategy);
            }

            foreach(var strategy in _physicsProcessStrategies)
            {
                entity.DataManager.LoadStrategyData(strategy);
                entity.StrategyManager.AddStrategy(strategy, StrategyGroup.PhysicsProcessStrategy);
            }
        }

        public void UnloadStrategies(IEntity entity)
        {
            foreach (var strategy in _strategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.NormalStrategy);
                entity.DataManager.UnloadStrategyData(strategy);
            }

            foreach (var strategy in _processStrategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.ProcessStrategy);
                entity.DataManager.UnloadStrategyData(strategy);
            }

            foreach (var strategy in _physicsProcessStrategies)
            {
                entity.StrategyManager.RemoveStrategy(strategy.GetType(), StrategyGroup.PhysicsProcessStrategy);
                entity.DataManager.UnloadStrategyData(strategy);
            }
        }

        public void Enter(IEntity entity)
        {
            if (entity is T typedEntity)
            {
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
                GD.Print($"{entity.EntityName} exit state: {GetType().Name}");
            }
            else
            {
                GD.PrintErr($"{entity.EntityName} can't fit in type: {typeof(T).FullName} when state enter");
            }
        }

        public abstract void Exit(T entity);

        public Tuple<Type, Action> Transit(IEntity entity, string token, params object[] parameters)
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

        public abstract Tuple<Type, Action> Transit(T entity, string token, params object[] parameters);

        public D AccessData<D>(IEntity entity) where D : BasicData
        {
            return entity.DataManager.Get<D>();
        }
    }
}
