using FlickrUploader.Core.Eventing;
using MediatR;

namespace FlickrUploader.Core.Mediator
{
    public class UnifiedMediator : IUnifiedMediator
    {
        private readonly IMediator _mediator;

        public UnifiedMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Execute(ICommand command)
        {
            _mediator.Send(command);
        }

        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            // TODO this might not be the best way to do it
            // see this - https://www.youtube.com/watch?v=bda13k0vfc0
            return _mediator.Send(command).Result;
        }

        public void Publish(IDomainEvent domainEvent)
        {
            _mediator.Publish(domainEvent);
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            return _mediator.Send(query).Result;
        }
    }
}