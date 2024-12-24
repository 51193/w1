using Godot;
using MyGame.Entity.Component;
using MyGame.Interface;
using MyGame.Manager;
using System.Collections.Generic;

namespace MyGame.Entity.MainBody
{
    public partial class BasicPlayer : BasicCharacter, IInteractableEntityScanner
    {
        [Export]
        public Area2D ScanningArea { get; set; }
        public List<IInteractableEntity> AccessibleInteratableEntities { get; init; } = new();

        public override void _Ready()
        {
            this.InitializeInteractableScanner();
            StageManager.Instance.GamePlayStage.SetCameraTarget(this);
        }

        public override void AfterInitialize()
        {
            InterfaceManager.Instance.InitializeInventoryInterface(this, 10);
        }
    }
}
