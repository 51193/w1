using Godot;
using MyGame.Manager;

namespace MyGame.Stage
{
    public partial class GamePlay : BasicStage
    {
        [Export]
        private MapTransition _mapTransition;
        [Export]
        private Camera2D _camera2D;

        private SaveConfig _saveConfig;
        private Node2D _cameraTarget;

        public void InitMap(SaveConfig config)
        {
            _saveConfig = config;
            SaveManager.Instance.Load(_saveConfig);
        }

        public void SetCameraTarget(Node2D node)
        {
            _cameraTarget = node;
        }

        public override void _Process(double delta)
        {
            if (_cameraTarget != null)
            {
                _camera2D.GlobalPosition = _cameraTarget.GlobalPosition;
            }

            if (Input.IsActionJustReleased("save"))
            {
                SaveManager.Instance.Save(_saveConfig);
            }

            if (Input.IsActionJustReleased("load"))
            {
                SaveManager.Instance.Load(_saveConfig);
            }
        }
    }
}
