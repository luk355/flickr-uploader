using UnifiedMediatR.Eventing;

namespace FlickrUploader.Core.Eventing
{
    public interface IAsyncDomainEventHandler<in TEvent> : IAsyncDomainEventHandler<TEvent, string> where TEvent : IAsyncDomainEvent
    {
    }
}