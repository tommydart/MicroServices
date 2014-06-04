namespace MicroServices.Proxy
{
    public interface IServiceExecutor
    {
        object Call(MessageContract contract);
    }
}