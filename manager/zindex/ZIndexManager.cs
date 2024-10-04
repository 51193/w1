using Godot;
using System.Collections.Generic;

namespace MyGame.Manager
{
	public partial class ZIndexManager : Node
	{
		private Dictionary<string, List<Node2D>> _canvasItems = new();
		private List<string> _resortNameNextFrame = new();

		[Signal]
		public delegate void IncludeNodeIntoRenderingOrderGroupEventHandler(string name, Node2D node);
		[Signal]
		public delegate void ClearNodeFromRenderingOrderGroupEventHandler(string name);
		[Signal]
		public delegate void ResortRenderingOrderEventHandler(string name);

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
			IncludeNodeIntoRenderingOrderGroup += IncludeNode;
			ClearNodeFromRenderingOrderGroup += ClearNode;
			ResortRenderingOrder += ResortNode;
		}

		public override void _ExitTree()
		{
			IncludeNodeIntoRenderingOrderGroup -= IncludeNode;
			ClearNodeFromRenderingOrderGroup -= ClearNode;
			ResortRenderingOrder -= ResortNode;
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
