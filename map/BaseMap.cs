using Godot;
using System.Collections.Generic;
using System.Linq;

namespace MyGame.Map
{
    public partial class BaseMap : Node
    {
        protected string _name = "BaseMap(shouldn't display)";

        protected Dictionary<string, LandmarkArea> _landmarks = new();

        public string GetMapName() {  return _name; }

        public Vector2 GetLandmarkPosition(string landmarkName)
        {
            if (_landmarks.ContainsKey(landmarkName))
            {
                return _landmarks[landmarkName].Position;
            }
            else
            {
                GD.PrintErr($"Landmark {landmarkName} not found!");
                return Vector2.Zero;
            }
        }

        private void GetLandmarks()
        {
            foreach (Node node in GetChildren())
            {
                if(node is LandmarkArea landmark)
                {
                    _landmarks[landmark.LandmarkName] = landmark;
                }
            }
        }

        public override void _Ready()
        {
            GetLandmarks();
        }
    }
}
