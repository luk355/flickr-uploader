using MediatR;

namespace FlickrUploader.Core.Eventing
{
    public interface IDomainEvent : INotification
    {
        string Id { get; }
    }
}