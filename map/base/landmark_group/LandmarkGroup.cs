using Godot;
using System.Collections.Generic;

namespace MyGame.Map
{
	public partial class LandmarkGroup : Node
	{
		private readonly Dictionary<string, LandmarkArea> _landmarks = new();

		private void GetLandmarks()
		{
			foreach (Node node in GetChildren())
			{
				if (node is LandmarkArea landmark)
				{
					if (!_landmarks.ContainsKey(landmark.Name))
					{
						_landmarks[landmark.LandmarkName] = landmark;
					}
					else
					{
						GD.PrintErr($"Duplicate LandmarkName when getting landmark: {landmark.LandmarkName}");
					}
				}
			}
		}

		public Vector2 GetLandmarkPosition(string landmarkName)
		{
			if (_landmarks.ContainsKey(landmarkName))
			{
				return _landmarks[landmarkName].Position;
			}
			else
			{
				GD.PrintErr($"Landmark {landmarkName} not found when getting position");
				return Vector2.Zero;
			}
		}

		public override void _Ready()
		{
			GetLandmarks();
		}
	}
}
