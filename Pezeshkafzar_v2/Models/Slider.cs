using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class Slider : BaseEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
