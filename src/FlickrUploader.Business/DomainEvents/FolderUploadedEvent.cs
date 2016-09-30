using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.DomainEvents
{
    public class FolderUploadedEvent : IAsyncDomainEvent
    {
        public string Id { get; set; }
    }
}