using Godot;

namespace MyGame.Util
{
    public static class GDUtil
    {
        public static void ClearAllSignalConnections(Node target, StringName signalName)
        {
            var connections = target.GetSignalConnectionList(signalName);

            foreach (Godot.Collections.Dictionary connection in connections)
            {
                var callable = (Callable)connection["callable"];

                target.Disconnect(signalName, callable);
            }
        }
    }
}
