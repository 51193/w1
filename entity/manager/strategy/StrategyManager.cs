using Godot;
using MyGame.Entity.MainBody;
using MyGame.Entity.Strategy;
using MyGame.Manager;
using System;
using System.Collections.Generic;

namespace MyGame.Entity.Manager
{
    public enum StrategyGroup
    {
        NormalStrategy = 0,
        ProcessStrategy,
        PhysicsProcessStrategy
    }

    internal class StrategyWithRefCount
    {
        public int RefCount;
        public IStrategy Strategy;

        public StrategyWithRefCount(IStrategy strategy)
        {
            RefCount = 0;
            Strategy = strategy;
        }
    }

    internal class SafeStrategyDictionary
    {
        private readonly Dictionary<Type, StrategyWithRefCount> _strategies = new();
        private readonly List<IStrategy> _delayAddStrategies = new();
        private readonly List<Type> _delayRemoveStrategies = new();

        public void AddStrategy(IStrategy strategy)
        {
            _delayAddStrategies.Add(strategy);
        }

        public void RemoveStrategy(Type type)
        {
            _delayRemoveStrategies.Add(type);
        }

        public void CallStrategies(IEntity entity, double dt = 0)
        {
            foreach (var strategy in _strategies.Values)
            {
                strategy.Strategy.Activate(entity, dt);
            }
        }

        public void ApplyDelayOperations()
        {
            foreach (var add in _delayAddStrategies)
            {
                Type type = add.GetType();
                if (!_strategies.ContainsKey(type))
                {
                    _strategies[type] = new StrategyWithRefCount(add);
                }
                _strategies[type].RefCount++;
            }
            _delayAddStrategies.Clear();

            foreach (var remove in _delayRemoveStrategies)
            {
                if (!_strategies.ContainsKey(remove))
                {
                    GD.PrintErr($"Can't remove strategy: {remove.FullName}, because it is not exist");
                }
                else
                {
                    _strategies[remove].RefCount--;
                    if (_strategies[remove].RefCount <= 0)
                    {
                        _strategies.Remove(remove);
                    }
                }
            }
            _delayRemoveStrategies.Clear();
        }
    }

    public class StrategyManager
    {
        private readonly IEntity _entity;

        private readonly Dictionary<Type, StrategyWithRefCount> _strategies = new();
        private readonly SafeStrategyDictionary _processStrategies = new();
        private readonly SafeStrategyDictionary _physicsProcessStrategies = new();

        public StrategyManager(IEntity entity) { _entity = entity; }

        private void AddToStrategy(IStrategy instance)
        {
            Type type = instance.GetType();
            if (!_strategies.ContainsKey(type))
            {
                _strategies[type] = new StrategyWithRefCount(instance);
            }
            _strategies[type].RefCount++;
        }

        private void RemoveFromStrategy(Type type)
        {
            if (!_strategies.ContainsKey(type))
            {
                GD.PrintErr($"Invalid strategy type to earse in StrategyManager: {type.FullName}");
                return;
            }
            _strategies[type].RefCount--;
            if (_strategies[type].RefCount <= 0)
            {
                _strategies.Remove(type);
            }
        }

        public void AddStrategy(IStrategy strategy, StrategyGroup strategyGroup)
        {
            switch (strategyGroup)
            {
                case StrategyGroup.NormalStrategy:
                    AddToStrategy(strategy);
                    break;
                case StrategyGroup.ProcessStrategy:
                    _processStrategies.AddStrategy(strategy);
                    break;
                case StrategyGroup.PhysicsProcessStrategy:
                    _physicsProcessStrategies.AddStrategy(strategy);
                    break;
            }
        }

        public void AddStrategy<T>(StrategyGroup strategyGroup) where T : IStrategy, new()
        {
            IStrategy instance = StrategyInstanceManager.Instance.GetInstance<T>();
            AddStrategy(instance, strategyGroup);
        }

        public void RemoveStrategy(Type type, StrategyGroup strategyGroup)
        {
            switch (strategyGroup)
            {
                case StrategyGroup.NormalStrategy:
                    RemoveFromStrategy(type);
                    break;
                case StrategyGroup.ProcessStrategy:
                    _processStrategies.RemoveStrategy(type);
                    break;
                case StrategyGroup.PhysicsProcessStrategy:
                    _physicsProcessStrategies.RemoveStrategy(type);
                    break;
            }
        }

        public void RemoveStrategy<T>(StrategyGroup strategyGroup) where T : IStrategy
        {
            Type type = typeof(T);
            RemoveStrategy(type, strategyGroup);
        }

        public void ActivateStrategy(Type type)
        {
            if (!_strategies.ContainsKey(type))
            {
                GD.PrintErr($"Activate invalid strategy in Strategy Manager: {type.FullName}");
                return;
            }
            _strategies[type].Strategy.Activate(_entity);
        }

        public void ActivateStrategy<T>() where T : IStrategy
        {
            Type type = typeof(T);
            ActivateStrategy(type);
        }

        public void Process(double dt)
        {
            _processStrategies.ApplyDelayOperations();
            _processStrategies.CallStrategies(_entity, dt);
        }

        public void PhysicsProcess(double dt)
        {
            _physicsProcessStrategies.ApplyDelayOperations();
            _physicsProcessStrategies.CallStrategies(_entity, dt);
        }
    }
}
