using MicroServices.Contracts;

namespace MicroServices.Server
{
    public class RequestResponseService : IRequestResponseService
    {
        public string Call(TestContract data1, TestContract data2)
        {
            return "Response " + data1.Text + " - " + data2.Text;
        }
    }
}