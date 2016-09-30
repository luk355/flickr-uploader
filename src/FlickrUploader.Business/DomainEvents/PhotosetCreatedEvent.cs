using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.DomainEvents
{
    public class PhotosetCreatedEvent : IDomainEvent
    {
        public string Id { get; set; }
    }
}