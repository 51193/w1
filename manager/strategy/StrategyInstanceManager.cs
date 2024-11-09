using Godot;
using MyGame.Strategy;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class StrategyInstanceManager : Node
	{
		private readonly Dictionary<Type, IStrategy> _strategyInstances = new();

		public T GetInstance<T>() where T : IStrategy, new()
		{
			Type type = typeof(T);
			if (!_strategyInstances.ContainsKey(type))
			{
				_strategyInstances[type] = new T();
			}
			return (T)_strategyInstances[type];
		}

        private static StrategyInstanceManager _instance;
        public static StrategyInstanceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GD.PrintErr("StrategyInstanceManager is not available");
                }
                return _instance;
            }
        }

        public override void _EnterTree()
        {
			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				GD.PrintErr("Duplicate StrategyInstanceManager entered the tree, this is not allowed");
			}
        }

        public override void _ExitTree()
        {
            if(_instance == this)
			{
				_instance = null;
			}
        }
    }
}
