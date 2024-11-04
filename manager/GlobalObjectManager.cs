using Godot;
using Godot.Collections;
using MyGame.Entity;
using MyGame.Interface;

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

		public static T HandleGlobalObject<T>(string objectName) where T : Node
		{
			if (GetGlobalObject(objectName) is T validateObject)
			{
				return validateObject;
			}
			else
			{
				GD.PrintErr($"Invalid {objectName}");
				return null;
			}
		}

		public static PackedScene GetResource(string name)
		{
			return HandleGlobalObject<ResourceManager>("ResourceManager").GetResource(name);
		}

		public static void EnterStage(string stageName)
		{
			HandleGlobalObject<StageManager>("StageManager").PushStage(stageName);
		}

		public static void ExitStage()
		{
			HandleGlobalObject<StageManager>("StageManager").PopStage();
		}

		public static MapTransition GetMapTransition()
		{
			return HandleGlobalObject<MapTransition>("MapTransition");
        }

		public static void TransitMap(string departureName, string exitName, Vector2 exitPosition, Node entity)
		{
			HandleGlobalObject<MapTransition>("MapTransition").TransitionProcess(departureName, exitName, exitPosition, (BasicDynamicEntity)entity);
		}

		public static void Save(string fileName)
		{
            HandleGlobalObject<MapTransition>("MapTransition").ToSaveData(fileName);
        }

		public static void Load(string fileName)
        {
            HandleGlobalObject<MapTransition>("MapTransition").FromSaveData(fileName);
        }

        public static void FocusOnCharacter(BasicCharacter character)
        {
			HandleGlobalObject<FocusedCharacterManager>("FocusedCharacterManager").FocusedCharacter = character;
        }

		public static BasicCharacter GetFocusedCharacter()
		{
			return HandleGlobalObject<FocusedCharacterManager>("FocusedCharacterManager").FocusedCharacter;
		}

		public static void InitializeInventoryInterface(BasicCharacter character, int visibleSlotCount)
		{
			HandleGlobalObject<InterfaceManager>("InterfaceManager").InitializeInventoryInterface(character, visibleSlotCount);
		}

		public static BasicItemPopupMenu GetItemPopupMenu(string itemPopupMenuName)
		{
			return HandleGlobalObject<InterfaceManager>("InterfaceManager").GetItemPopupMenu(itemPopupMenuName);
        }
    }
}
