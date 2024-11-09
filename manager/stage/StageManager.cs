using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class StageManager : Node
	{
		private readonly Stack<PackedScene> _stageStack = new();
		private Node _currentStage;

		public void PushStage(string name)
		{
			PackedScene scene = ResourceManager.Instance.GetResource(name);

			if (_currentStage != null)
			{
				_currentStage.QueueFree();
				_currentStage = null;
			}

			_stageStack.Push(scene);
			_currentStage = scene.Instantiate();
			AddChild(_currentStage);

			GD.Print($"Stage stack have {_stageStack.Count} instance(s) inside");
		}

		public void PopStage()
		{
			if (_currentStage != null)
			{
				_currentStage.QueueFree();
				_currentStage = null;
				if (_stageStack.Count > 0)
				{
					_stageStack.Pop();
				}
			}

			if (_stageStack.Count > 0)
			{
				_currentStage = _stageStack.Peek().Instantiate();
				AddChild(_currentStage);
			}

			GD.Print($"Stage stack have {_stageStack.Count} instance(s) inside");
		}

		private static StageManager _instance;
		public static StageManager Instance
		{
			get
			{
				if (_instance == null)
                {
                    GD.PrintErr("StageManager is not available");
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
                GD.PrintErr("Duplicate StageManager entered the tree, this is not allowed");
            }
        }

		public override void _ExitTree()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

		public override void _Ready()
		{
			PushStage("MainMenu");
		}
	}
}
