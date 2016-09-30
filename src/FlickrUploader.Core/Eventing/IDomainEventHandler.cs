using UnifiedMediatR.Eventing;

namespace FlickrUploader.Core.Eventing
{
    public interface IDomainEventHandler<in TEvent> : IDomainEventHandler<TEvent, string> where TEvent : IDomainEvent
    {
    }
}