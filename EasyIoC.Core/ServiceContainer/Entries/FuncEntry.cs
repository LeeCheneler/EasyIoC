using System;

namespace EasyIoC.Core.ServiceContainer.Entries
{
    internal class FuncEntry : IEntry
    {
        public FuncEntry(Func<object> func)
        {
            _func = func;
        }


        public object GetService()
        {
            return _func.Invoke();
        }


        private readonly Func<object> _func;
    }
}
