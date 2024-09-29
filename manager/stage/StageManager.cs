using Godot;
using System;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class StageManager : Node
	{
		private readonly Stack<PackedScene> _stageStack = new();
		private Node _currentStage;

		[Signal]
		public delegate void EnterStageEventHandler(String stageName);
		[Signal]
		public delegate void ExitStageEventHandler();

		private void PushStage(String name)
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

		private void PopStage()
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
			EnterStage += PushStage;
			ExitStage += PopStage;
		}

		public override void _ExitTree()
		{
			EnterStage -= PushStage;
			ExitStage -= PopStage;
			GlobalObjectManager.RemoveGlobalObject("StageManager");
		}

		public override void _Ready()
		{
			EmitSignal(SignalName.EnterStage, "MainMenuStage");
		}
	}
}
