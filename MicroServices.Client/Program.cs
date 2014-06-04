using System;
using System.Net;
using System.Threading.Tasks;
using Griffin.Net;
using Griffin.Net.Protocols.MicroMsg;
using MicroServices.Contracts;
using MicroServices.Proxy;
using StructureMap;

namespace MicroServices.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Process().Wait();
            Console.ReadKey();
        }

        private async static Task Process()
        {
            using (var client = new ChannelTcpClient<object>(
                new MicroMessageEncoder(new JsonSerializer()),
                new MicroMessageDecoder(new JsonSerializer())))
            {

                ObjectFactory.Configure(x => x.Scan(z =>
                {
                    z.AssemblyContainingType<IRequestResponseService>();
                    z.With(new ServiceFactoryConvention(new ServiceExecutor(client)));
                    z.AddAllTypesOf<IMicroService>();
                }));


                await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 1234);

                var srv = ObjectFactory.GetInstance<IRequestResponseService>();

                var result = srv.Call(
                    new TestContract {Text = "Hello text 1"},
                    new TestContract {Text = "Hello text 2"});

                Console.WriteLine(result);

                await client.CloseAsync();
            }
        }
    }
}
