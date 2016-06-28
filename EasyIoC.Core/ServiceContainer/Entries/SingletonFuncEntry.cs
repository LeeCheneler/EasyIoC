using System;

namespace EasyIoC.Core.ServiceContainer.Entries
{
    public class SingletonFuncEntry : IEntry
    {
        public SingletonFuncEntry(Func<object> func)
        {
            _func = func;
        }


        public object GetService()
        {
            if (_instance == null)
            {
                _instance = _func.Invoke();
            }

            return _instance;
        }


        private readonly Func<object> _func;
        private object _instance = null;
    }
}