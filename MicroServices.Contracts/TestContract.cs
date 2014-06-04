using System;

namespace MicroServices.Contracts
{
    [Serializable]
    public class TestContract
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
    }
}
