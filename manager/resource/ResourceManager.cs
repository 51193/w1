using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MyGame.Manager
{
	public class ResourcePath
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string Description { get; set; }
	}

	public partial class ResourceManager : Node
	{
		private Dictionary<string, ResourcePath> _pathDict;

		private void LoadResourcePaths(string path)
		{
			try
			{
				string jsonContent = File.ReadAllText(path);
                List<ResourcePath> resources = JsonSerializer.Deserialize<List<ResourcePath>>(jsonContent);

				_pathDict = resources.ToDictionary(
					t => t.Name,
					t => t
					);

				foreach (var resource in resources)
				{
					GD.Print($"{resource.Name}-{resource.Path}: {resource.Description}");
				}
			}
			catch (Exception e)
			{
				GD.PrintErr($"Failed to load resource path: {e.Message}");
				return;
			}
		}

		public string GetResourcePath(string name)
		{
			if (_pathDict.TryGetValue((name), out var path))
			{
				return path.Path;
			}
			else
			{

				GD.PrintErr($"Resource not found: {name}");
				return null;
			}
		}

		public PackedScene GetResource(string name)
		{
			string resourcePath = GetResourcePath(name);
			if (string.IsNullOrEmpty(resourcePath))
			{
				return null;
			}

			PackedScene scene = GD.Load<PackedScene>(resourcePath);
			if (scene == null)
			{
				GD.PrintErr($"Failed to load resource from {resourcePath}");
				return null;
			}

			return scene;
		}

		public override void _EnterTree()
		{
			GlobalObjectManager.AddGlobalObject("ResourceManager", this);
			LoadResourcePaths("path.json");
		}

        public override void _ExitTree()
        {
			GlobalObjectManager.RemoveGlobalObject("ResourceManager");
        }

        public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
		}
	}
}
