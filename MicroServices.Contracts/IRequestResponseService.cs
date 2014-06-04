namespace MicroServices.Contracts
{
    public interface IRequestResponseService : IMicroService
    {
        string Call(TestContract data1, TestContract data2);
    }
}
