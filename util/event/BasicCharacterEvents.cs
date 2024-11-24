using MyGame.Entity;
using MyGame.Entity.State;
using System;

namespace MyGame.Util
{
    public class BasicCharacterEvents : IEventProvider
    {
        public Action GetEvent(string key, params object[] parameters)
        {
            switch (key)
            {
                case "ChangeControlStateToHardwareInputControlState":
                    {
                        if (parameters.Length > 0 && parameters[0] is IEntity entity)
                        {
                            return () =>
                            {
                                entity.StateManager.Transit("Input", "HardwareInput");
                            };
                        }
                        break;
                    }
            }
            return () => { };
        }
    }
}
