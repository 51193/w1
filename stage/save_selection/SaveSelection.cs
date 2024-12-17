using Godot;
using MyGame.Manager;

namespace MyGame.Stage
{
	public partial class SaveSelection : BasicStage
	{
		[Export]
		private HBoxContainer _container;
		[Export]
		private PackedScene _saveSlot;

		private void DisplaySaveSlot()
		{
			foreach (Node slot in _container.GetChildren())
			{
				slot.QueueFree();
			}

			foreach (var slotConfig in SaveManager.Instance.SaveConfigs)
			{
				var instance = _saveSlot.Instantiate<SaveSlot>();
				if (slotConfig.IsNew)
				{
					instance.Text = slotConfig.Title + "\n" + "xxxx-xx-xx xx:xx:xx" + "\n";
				}
				else
				{
					instance.Text = slotConfig.Title + "\n" + slotConfig.Timestamp + "\n";
				}
				instance.Pressed += () => OnSaveSlotPressed(slotConfig);
				_container.AddChild(instance);
			}
		}

		private static void OnSaveSlotPressed(SaveConfig config)
		{
			StageManager.Instance.PushStage("GamePlay");
			StageManager.Instance.GamePlayStage.InitMap(config);
		}

		public override void _Ready()
		{
			DisplaySaveSlot();
		}
	}
}
