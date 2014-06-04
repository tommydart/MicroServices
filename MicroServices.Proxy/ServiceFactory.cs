using System;
using Castle.DynamicProxy;

namespace MicroServices.Proxy
{
    public class ServiceFactory
    {
        public static object Create(IServiceExecutor client, Type serviceType)
        {
            if(client == null) throw new ArgumentNullException("client");
            if(!serviceType.IsInterface) throw new Exception("Only interface allowed.");

            var gen = new ProxyGenerator();
            var proxy = gen.CreateInterfaceProxyWithoutTarget(
                serviceType,
                Type.EmptyTypes,
                new ServiceActionInterceptor(client, serviceType));

            return proxy;
        }
    }

    internal class ServiceActionInterceptor : IInterceptor
    {
        private readonly IServiceExecutor _executor;
        private readonly Type _serviceType;

        public ServiceActionInterceptor(IServiceExecutor executor, Type serviceType)
        {
            _executor = executor;
            _serviceType = serviceType;
        }

        public void Intercept(IInvocation invocation)
        {
            var message = new MessageContract(_serviceType.AssemblyQualifiedName, invocation.Method.Name, invocation.Arguments);
            invocation.ReturnValue = _executor.Call(message);
        }
    }
}