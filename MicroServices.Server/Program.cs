using System;
using System.Diagnostics;
using System.Net;
using Griffin.Net;
using Griffin.Net.Channels;
using Griffin.Net.Protocols.MicroMsg;
using Griffin.Net.Protocols.MicroMsg.Server;
using MicroServices.Proxy;
using StructureMap;
using StructureMap.Graph;

namespace MicroServices.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ObjectFactory.Configure(x => x.Scan(z =>
            {
                z.TheCallingAssembly();
                z.WithDefaultConventions();
                z.RegisterConcreteTypesAgainstTheFirstInterface();
            }));

            var settings = new ChannelTcpListenerConfiguration(
                () => new MicroMessageDecoder(new JsonSerializer()),
                () => new MicroMessageEncoder(new JsonSerializer()));

            var server = new MicroMessageTcpListener(settings)
            {
                MessageReceived = OnServerMessage
            };
            server.Start(IPAddress.Any, 1234);

            Console.WriteLine("Server started. Press any key to exit...");
            Console.ReadKey();
        }

        private static void OnServerMessage(ITcpChannel channel, object message)
        {
            var contract = message as MessageContract;
            if (contract != null)
            {
                var type = Type.GetType(contract.ServiceType);
                if (type != null)
                {
                    var srv = ObjectFactory.GetInstance(type);
                    var result = type.GetMethod(contract.MethodName).Invoke(srv, contract.Arguments); // find better/faster way
                    channel.Send(result);
                }
            }
            Debug.WriteLine("Received message type: " + message.GetType().Name);
        }
    }
}
