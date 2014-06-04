using System;

namespace MicroServices.Proxy
{
    [Serializable]
    public class MessageContract
    {
        public string ServiceType { get; set; }
        public string MethodName { get; set; }
        public object[] Arguments { get; set; }

        public MessageContract(string serviceType, string methodName, params object[] arguments)
        {
            ServiceType = serviceType;
            MethodName = methodName;
            Arguments = arguments;
        }
    }
}