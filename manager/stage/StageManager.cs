using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class StageManager : Node
	{
		private readonly Stack<PackedScene> _stageStack = new();
		private Node _currentStage;

		public void PushStage(String name)
		{
			PackedScene scene = GlobalObjectManager.GetResource(name);

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

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("StageManager", this);
		}

		public override void _ExitTree()
		{
			GlobalObjectManager.RemoveGlobalObject("StageManager");
		}

		public override void _Ready()
		{
			PushStage("MainMenu");
		}
	}
}
