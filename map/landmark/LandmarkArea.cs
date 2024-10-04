using Godot;
using System;

namespace MyGame.Map
{
	public partial class LandmarkArea : Node2D
	{
		[Export]
		public string LandmarkName = "LandMark";
		[Export]
		public string AniamtionPlayedAfterSpawn;
	}
}
