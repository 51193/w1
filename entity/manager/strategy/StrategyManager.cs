using Godot;
using MyGame.Manager;
using MyGame.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.Entity
{
    public enum StrategyGroup
    {
        NormalStrategy = 0,
        ProcessStrategy,
        PhysicsProcessStrategy
    }

    internal class SafeStrategyDictionary
    {
        private readonly Dictionary<Type, IStrategy> _strategies = new();
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
                strategy.Activate(entity, dt);
            }
        }

        public void ApplyDelayOperations()
        {
            foreach (var add in _delayAddStrategies)
            {
                Type type = add.GetType();
                if (_strategies.ContainsKey(type))
                {
                    GD.PrintErr($"Add duplicate strategy: {type.FullName}");
                }
                else
                {
                    _strategies[type] = add;
                }
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
                    _strategies.Remove(remove);
                }
            }
            _delayRemoveStrategies.Clear();
        }
    }

    public class StrategyManager
    {
        private readonly IEntity _entity;

        private readonly Dictionary<Type, IStrategy> _strategies = new();
        private readonly SafeStrategyDictionary _processStrategies = new();
        private readonly SafeStrategyDictionary _physicsProcessStrategies = new();

        public StrategyManager(IEntity entity) { _entity = entity; }

        private void InsertToStrategy(IStrategy instance)
        {
            Type type = instance.GetType();
            if (_strategies.ContainsKey(type))
            {
                GD.PrintErr($"Duplicate strategy type in StrategyManager: {type.FullName}");
                return;
            }
            _strategies[type] = instance;
        }

        private void EraseFromStrategy(Type type)
        {
            if (!_strategies.ContainsKey(type))
            {
                GD.PrintErr($"Invalid strategy type to earse in StrategyManager: {type.FullName}");
                return;
            }
            _strategies.Remove(type);
        }

        public void AddStrategy(IStrategy strategy, StrategyGroup strategyGroup)
        {
            switch (strategyGroup)
            {
                case StrategyGroup.NormalStrategy:
                    InsertToStrategy(strategy);
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
                    EraseFromStrategy(type);
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
            _strategies[type].Activate(_entity);
        }

        public void ActivateStrategy<T>() where T : IStrategy
        {
            Type type = typeof(T);
            ActivateStrategy(type);
        }

        public void Process(double dt)
        {
            _processStrategies.CallStrategies(_entity, dt);
            _processStrategies.ApplyDelayOperations();
        }

        public void PhysicsProcess(double dt)
        {
            _physicsProcessStrategies.CallStrategies(_entity, dt);
            _physicsProcessStrategies.ApplyDelayOperations();
        }
    }
}
