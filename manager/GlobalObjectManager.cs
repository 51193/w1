using Godot;
using Godot.Collections;
using MyGame.Entity;

namespace MyGame.Manager
{
	public partial class GlobalObjectManager : Node
	{
		private static GlobalObjectManager _instance;
		public static GlobalObjectManager Instance => _instance;

		private readonly Dictionary<string, Node> _globalNodes = new();

		public GlobalObjectManager()
		{
			_instance ??= this;
		}

		public static void AddGlobalObject(string name, Node node)
		{
			if (!_instance._globalNodes.ContainsKey(name))
			{
				_instance._globalNodes[name] = node;
				GD.Print($"{name} have added to GlobalObjectManager");
			}
			else
			{
				GD.PrintErr($"Duplicate global object name: {name}");
			}
		}

		public static void RemoveGlobalObject(string name)
		{
			if (_instance._globalNodes.ContainsKey(name))
			{
				_instance._globalNodes.Remove(name);
			}
			else
			{
				GD.PrintErr($"{name} doesn't exist in GlobalObjectManager, unable to remove");
			}
		}

		public static Node GetGlobalObject(string name)
		{
			if (_instance._globalNodes.TryGetValue(name, out Node node))
			{
				return node;
			}
			else
			{
				GD.PrintErr($"Invalid global object name: {name}");
				return null;
			}
		}

		public static PackedScene GetResource(string name)
		{
			if (GetGlobalObject("ResourceManager") is ResourceManager resourceManager)
			{
				return resourceManager.GetResource(name);
			}
			else
			{
				GD.PrintErr("Invalid ResourceManager");
				return null;
			}
		}

		public static void EnterStage(string stageName)
		{
			if (GetGlobalObject("StageManager") is StageManager stageManager)
			{
				stageManager.PushStage(stageName);
			}
			else
			{
				GD.PrintErr("Invalid StageManager");
			}
		}

		public static void ExitStage()
		{
			if (GetGlobalObject("StageManager") is StageManager stageManager)
			{
				stageManager.PopStage();
			}
			else
			{
				GD.PrintErr("Invalid StageManager");
			}
		}

		public static void TransitMap(string departureName, string exitName, Vector2 exitPosition, Node entity)
		{
			if (GetGlobalObject("MapTransition") is MapTransition mapTransition)
			{
				mapTransition.TransitionProcess(departureName, exitName, exitPosition, (BaseDynamicEntity)entity);
			}
			else
			{
				GD.PrintErr("Invalid MapTransition");
			}
		}

		public static void IncludeNodeIntoRenderingOrderGroup(string name, Node2D canvasItem)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.IncludeNode(name, canvasItem);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
		}

		public static void ClearNodeInRenderingOrderGroup(string name)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.ClearNode(name);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
		}

		public static void ResortRenderingOrder(string name)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.ResortNode(name);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
        }

        public static void FocusOnCharacter(BaseCharacter character)
        {
            if (GetGlobalObject("FocusedCharacterManager") is FocusedCharacterManager focusedCharacterManager)
            {
                focusedCharacterManager.FocusOnCharacter(character);
            }
            else
            {
                GD.PrintErr("Invalid FocusedCharacterManager");
            }
        }

		public static BaseCharacter GetFocusedCharacter()
		{

			if (GetGlobalObject("FocusedCharacterManager") is FocusedCharacterManager focusedCharacterManager)
			{
				return focusedCharacterManager.GetFocusedCharacter();
			}
			else
			{
				GD.PrintErr("Invalid FocusedCharacterManager");
				return null;
			}
		}
    }
}
