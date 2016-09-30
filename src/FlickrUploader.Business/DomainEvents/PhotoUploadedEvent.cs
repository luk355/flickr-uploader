using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.DomainEvents
{
    public class PhotoUploadedEvent : IAsyncDomainEvent
    {
        public string Id { get; set; }

        public string PhotoSet { get; set; }
    }
}