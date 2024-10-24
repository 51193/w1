﻿using Godot;
using System.Collections.Generic;

namespace MyGame.Map
{
    public partial class BaseMap : Node2D
    {
        protected string _name;

        protected Dictionary<string, LandmarkArea> _landmarks = new();

        public BaseMap()
        {
            _name = GetType().Name;
        }

        public string GetMapName() {  return _name; }

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
