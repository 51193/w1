using System;

namespace MyGame.Util
{
    public interface IEventProvider
    {
        public Action GetEvent(string eventName, params object[] parameters);
    }
}
