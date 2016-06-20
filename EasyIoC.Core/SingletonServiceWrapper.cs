namespace EasyIoC.Core
{
    public class SingletonServiceWrapper
    {
        public SingletonServiceWrapper(ServiceActivator activator)
        {
            _activator = activator;
        }


        public object Get()
        {
            if (_instance == null)
            {
                _instance = _activator.Invoke();
            }

            return _instance;
        }


        private ServiceActivator _activator;
        private object _instance = null;
    }
}
