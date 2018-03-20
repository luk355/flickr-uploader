using MediatR;

namespace FlickrUploader.Core.Eventing
{
    public interface IAsyncDomainEvent : INotification
    {
        string Id { get; }
    }
}