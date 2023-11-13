using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class CallReport: BaseEntity
    {
        public string Username { get; set; }
        public System.DateTime Date { get; set; }
        public string Ip { get; set; }
        public string Operator { get; set; }
    }
}
