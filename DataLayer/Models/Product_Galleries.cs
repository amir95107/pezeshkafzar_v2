using DataLayer.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{

    public partial class Product_Galleries : GuidAuditableEntity
    {
        [Required]
        public Guid ProductID { get; set; }
        [DisplayName("فایل")]
        public string ImageName { get; set; }
        [Required]
        [DisplayName("عنوان")]
        public string Title { get; set; }
        public GalleryType GalleryType { get; set; }
        //public bool IsMain { get; set; }

        public virtual Products Products { get; set; }

        protected override void EnsureReadyState(object @event)
        {
            throw new NotImplementedException();
        }

        protected override void EnsureValidState()
        {
            throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }

    public enum GalleryType
    {
        Image=0,
        Video=1
    }
}