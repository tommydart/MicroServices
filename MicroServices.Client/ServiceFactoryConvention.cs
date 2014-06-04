using System;
using System.Diagnostics;
using MicroServices.Proxy;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace MicroServices.Client
{
    internal class ServiceFactoryConvention : IRegistrationConvention
    {
        private readonly IServiceExecutor _executor;

        public ServiceFactoryConvention(IServiceExecutor executor)
        {
            _executor = executor;
        }

        public void Process(Type type, Registry registry)
        {
            Debug.WriteLine(type.Name);
            if(!type.IsInterface) return;
            var proxy = ServiceFactory.Create(_executor, type);
            registry.For(type).LifecycleIs(new TransientLifecycle()).Use(proxy);
        }
    }
}