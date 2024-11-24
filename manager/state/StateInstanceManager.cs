using Godot;
using MyGame.Entity.State;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class StateInstanceManager : Node
	{
		private readonly Dictionary<Type, IState> _stateInstances = new();

		public IState GetInstance(Type type)
		{
			if (!_stateInstances.ContainsKey(type))
			{
				_stateInstances[type] = (IState)Activator.CreateInstance(type);
			}
			return _stateInstances[type];
		}

		private static StateInstanceManager _instance;
		public static StateInstanceManager Instance
		{
			get
			{
				if (_instance == null)
				{
					GD.PrintErr("StateInstanceManager is not available");
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
				GD.PrintErr("Duplicate StateInstanceManager entered the tree, this is not allowed");
			}
		}

		public override void _ExitTree()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}
	}
}
