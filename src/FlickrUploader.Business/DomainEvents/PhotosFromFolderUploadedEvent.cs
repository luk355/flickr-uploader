using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.DomainEvents
{
    public class PhotosFromFolderUploadedEvent : IDomainEvent
    {
        public string Id { get; set; }
    }
}