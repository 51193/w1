﻿using MyGame.Entity;
using System;

namespace MyGame.Manager
{
    public static class MapTransitionEvents
    {
        public static Action GetEvent(string key, params object[] parameters)
        {
            switch (key)
            {
                case "InvokeManagers":
                    {
                        if (parameters.Length > 2 &&
                            parameters[0] is MapTransition mapTransition &&
                            parameters[1] is TransitionInfo transitionInfo &&
                            parameters[2] is BaseDynamicEntity dynamicEntity)
                        {
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
