using Godot;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class ZIndexManager : Node
	{
		private readonly Dictionary<string, List<Node2D>> _canvasItems = new();
		private readonly List<string> _resortNameNextFrame = new();

		public void IncludeNode(string name, Node2D node)
		{
			if(!_canvasItems.ContainsKey(name))
			{
				_canvasItems[name] = new();
			}
			_canvasItems[name].Add(node);
			ResortNode(name);
			GD.Print($"Include a node in rendering order sort, now have {_canvasItems[name].Count} node(s) inside");
		}

		public void ClearNode(string name)
		{
			if (_canvasItems.ContainsKey(name))
			{
				_canvasItems[name].Clear();
				GD.Print($"Group removed in ZIndexManager: {name}.");
			}
			else
			{
				GD.PrintErr($"Group already removed or does't exist in ZIndexManager: {name}.");
			}
		}

		public void ResortNode(string name)
		{
			if(!_resortNameNextFrame.Contains(name))
			{
				_resortNameNextFrame.Add(name);
			}
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("ZIndexManager", this);
		}

		public override void _ExitTree()
		{
			GlobalObjectManager.RemoveGlobalObject("ZIndexManager");
		}

		public override void _Process(double delta)
		{
			if (_resortNameNextFrame.Count > 0)
			{
				foreach (var name in _resortNameNextFrame)
				{
					if(_canvasItems.TryGetValue(name, out var list))
					{
						list.Sort((a, b) =>
						{
							return a.Position.Y.CompareTo(b.Position.Y);
						});
						int zIndex = 16;
						foreach (var item in list)
						{
							if (zIndex < 2048)
							{
								item.ZIndex = zIndex++;
							}
							else
							{
								GD.PrintErr($"Too many nodes in ZIndexManager: {name}");
							}
						}
					}
					else
					{
						GD.PrintErr($"{name} didn't exist in ZIndexManager");
					}
				}
				_resortNameNextFrame.Clear();
			}
		}
	}
}
