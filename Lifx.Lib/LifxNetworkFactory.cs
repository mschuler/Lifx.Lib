namespace Lifx.Lib
{
    public static class LifxNetworkFactory
    {
        private static readonly object _instanceLock = new object();
        private static LifxNetwork _instance;

        public static ILifxNetwork Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LifxNetwork();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}