using System;
using static MyGame.Entity.DynamicEntity0;

namespace MyGame.Entity
{
    public static class BaseCharacterEvents
    {
        public static Action GetEvent(string key, params object[] parameters)
        {
            switch(key)
            {
                case "ChangeControlStateToHardwareInputControlState":
                    {
                        if (parameters.Length > 0 && parameters[0] is IEntity entity)
                        {
                            return () => { entity.StateManager.ChangeState("ControlState", new HardwareInputControlState()); };
                        }
                        break;
                    }
            }
            return () => { };
        }
    }
}
