using System;
using Griffin.Net;
using MicroServices.Proxy;

namespace MicroServices.Client
{
    public class ServiceExecutor : IServiceExecutor
    {
        private readonly ChannelTcpClient<object> _client;
        private const int ConfigurableTimeout = 1000;

        public ServiceExecutor(ChannelTcpClient<object> client)
        {
            _client = client;
        }

        public object Call(MessageContract contract)
        {
            if (_client.SendAsync(contract).Wait(ConfigurableTimeout))
            {
                return _client.ReceiveAsync().Result;
            }
            throw new Exception(string.Format("Error invoking {0} remote service method.", contract.MethodName));
        }
    }
}