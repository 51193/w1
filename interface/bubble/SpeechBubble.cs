using Godot;

namespace MyGame.Interface
{
	public partial class SpeechBubble : Control
	{
		[Export]
		private Label _speechLabel;
		[Export]
		private Timer _speechTimer;

		public void ShowSpeech(string text, float duration = 2)
		{
			_speechLabel.Text = text;
			_speechTimer.WaitTime = duration;
			_speechTimer.Start();

			Visible = true;
		}

		public void HideSpeech()
		{
			Visible = false;
		}

		public override void _Ready()
		{
			_speechTimer.Timeout += HideSpeech;
		}
	}
}
