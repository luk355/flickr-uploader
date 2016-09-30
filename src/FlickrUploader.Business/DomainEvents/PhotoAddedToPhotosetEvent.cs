using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.DomainEvents
{
    public class PhotoAddedToPhotosetEvent : IDomainEvent
    {
        public string Id { get; set; }

        public string PhotosetId { get; set; }
    }
}