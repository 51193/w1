using Godot;
using MyGame.Entity;
using MyGame.Manager;
using System;

namespace MyGame.Util
{
    public class MapTransitionEvents : IEventProvider
    {
        public Action GetEvent(string key, params object[] parameters)
        {
            switch (key)
            {
                case "InvokeManagers":
                    {
                        if (parameters.Length > 1 &&
                            parameters[0] is BasicDynamicEntity dynamicEntity &&
                            parameters[1] is TransitionInfo transitionInfo)
                        {
                            MapTransition mapTransition = GlobalObjectManager.GetMapTransition();
                            return () =>
                            {
                                mapTransition.CallDeferred(
                                    nameof(mapTransition.InvokeManagers),
                                    transitionInfo.Destination,
                                    transitionInfo.EntryFrom,
                                    transitionInfo.EntryTo,
                                    dynamicEntity);
                            };
                        }
                        break;
                    }
            }
            return () => { };
        }
    }
}
