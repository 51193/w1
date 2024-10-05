using Godot;
using Godot.Collections;
using MyGame.Entity;

namespace MyGame.Manager
{
	public partial class GlobalObjectManager : Node
	{
		private static GlobalObjectManager _instance;
		public static GlobalObjectManager Instance => _instance;

		private Dictionary<string, Node> _globalNodes;

		public override void _EnterTree()
		{
			_instance ??= this;
			_globalNodes = new Dictionary<string, Node>();
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

		public static void EmitEnterStageSignal(string stageName)
		{
			if (GetGlobalObject("StageManager") is StageManager stageManager)
			{
				stageManager.EmitSignal(nameof(stageManager.EnterStage), stageName);
			}
			else
			{
				GD.PrintErr("Invalid StageManager");
			}
		}

		public static void EmitExitStageSignal()
		{
			if (GetGlobalObject("StageManager") is StageManager stageManager)
			{
				stageManager.EmitSignal(nameof(stageManager.ExitStage));
			}
			else
			{
				GD.PrintErr("Invalid StageManager");
			}
		}

		public static void EmitTransitMapSignal(string departureName, string exitName, Vector2 exitPosition, Node entity)
		{
			if (GetGlobalObject("MapTransition") is MapTransition mapTransition)
			{
				mapTransition.EmitSignal(nameof(mapTransition.TransitMap), departureName, exitName, exitPosition, entity);
			}
			else
			{
				GD.PrintErr("Invalid MapTransition");
			}
		}

		public static void EmitIncludeNodeIntoRenderingOrderGroupSignal(string name, Node2D canvasItem)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.EmitSignal(nameof(zIndexManager.IncludeNodeIntoRenderingOrderGroup), name, canvasItem);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
		}
		public static void EmitClearNodeFromRenderingOrderGroupSignal(string name)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.EmitSignal(nameof(zIndexManager.ClearNodeFromRenderingOrderGroup), name);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
		}

		public static void EmitResortRenderingOrderSignal(string name)
		{
			if (GetGlobalObject("ZIndexManager") is ZIndexManager zIndexManager)
			{
				zIndexManager.EmitSignal(nameof(zIndexManager.ResortRenderingOrder), name);
			}
			else
			{
				GD.PrintErr("Invalid ZIndexManager");
			}
		}

        public static void EmitRegistrateInteractablePairSignal(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
        {
            if (GetGlobalObject("InteractManager") is InteractManager interactManager)
            {
                interactManager.EmitSignal(nameof(interactManager.RegistrateInteractablePair), dynamicEntity, staticEntity);
            }
            else
            {
                GD.PrintErr("Invalid InteractManager");
            }
        }

        public static void EmitUnregistrateInteractablePairSignal(BaseInteractableDynamicEntity dynamicEntity, BaseInteractableStaticEntity staticEntity)
        {
            if (GetGlobalObject("InteractManager") is InteractManager interactManager)
            {
                interactManager.EmitSignal(nameof(interactManager.UnregistrateInteractablePair), dynamicEntity, staticEntity);
            }
            else
            {
                GD.PrintErr("Invalid InteractManager");
            }
        }
    }
}
