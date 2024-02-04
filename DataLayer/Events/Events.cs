using DataLayer.Models.Base;

namespace DataLayer.Models.Events
{
    public enum EventType
    {
        Add=1,
        Update=2,
        Delete=3
    }
    public class ProductGalleryChanged : DomainEvent
    {
        public string Title { get; set; }
        public Guid GalleryId { get; set; }
        public Guid ProductId { get; set; }
        public string ImageName { get; set; }
        public GalleryType GalleryType { get; set; }
    }

    public class ProductFeatureChanged : DomainEvent
    {
        public Guid FeatureId { get; set; }
        public Guid ProductId { get; set; }
        public string Value { get; set; }
    }

    public class ProductBrandChanged : DomainEvent
    {
        public Guid BrandId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class ProductSelectedGroupChanged : DomainEvent
    {
        public Guid GroupId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class ProductTagChanged : DomainEvent
    {
        public string Tag { get; set; }
        public Guid ProductId { get; set; }
    }

    public class MainImageSet : DomainEvent
    {
        public Guid GalleryId { get; set; }
        public Guid ProductId { get; set; }
    }
}
