using System;

namespace MyGame.Component
{
    public class LazyLoader<T>
    {
        private T _instance;
        private readonly Func<T> _factory;

        public LazyLoader(Func<T> factory)
        {
            _factory = factory;
        }

        public T Value
        {
            get
            {
                _instance ??= _factory();
                return _instance;
            }
        }

        public bool IsInitialized => _instance != null;

        public void Invoke(Action<T> action)
        {
            action(Value);
        }

        public TResult Invoke<TResult>(Func<T, TResult> func)
        {
            return func(Value);
        }
    }
}
