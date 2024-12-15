using Godot;
using System.Collections.Generic;

namespace MyGame.Entity.Component
{
    public interface IInteractableEntityScanner
    {
        public Area2D ScanningArea { get; set; }
        public List<IInteractableEntity> AccessibleInteratableEntities { get; init; }
    }

    public static class InteractableEntityScannerExtensions
    {
        public static void InitializeInteractableScanner(this IInteractableEntityScanner scanner)
        {
            if (scanner.ScanningArea == null)
            {
                GD.PrintErr("ScanningArea is not set in InteractableEntityScanner");
                return;
            }

            scanner.ScanningArea.BodyEntered += scanner.OnBodyEntered;
            scanner.ScanningArea.BodyExited += scanner.OnBodyExited;
        }

        public static void OnBodyEntered(this IInteractableEntityScanner scanner, Node node)
        {
            if (node is IInteractableEntity interactable)
            {
                scanner.AccessibleInteratableEntities.Add(interactable);
                GD.Print($"{interactable.GetType().Name} enter the {scanner.GetType().Name}'s ScanningArea");
            }
        }

        public static void OnBodyExited(this IInteractableEntityScanner scanner, Node node)
        {
            if (node is IInteractableEntity interactable)
            {
                interactable.HideTip();
                scanner.AccessibleInteratableEntities.Remove(interactable);
                GD.Print($"{interactable.GetType().Name} exit the {scanner.GetType().Name}'s ScanningArea");
            }
        }
    }
}
