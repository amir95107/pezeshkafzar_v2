using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class ContactForm : BaseEntity
    {
        public Guid? UserID { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Answer { get; set; }
        public bool IsFaq { get; set; }

        public virtual Users Users { get; set; }
    }
}
