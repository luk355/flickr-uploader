using FlickrUploader.Core.Eventing;
using MediatR;
using System.Threading.Tasks;

namespace FlickrUploader.Core.Mediator
{
    public class UnifiedMediator : IUnifiedMediator
    {
        private readonly IMediator _mediator;

        public UnifiedMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Execute(ICommand command)
        {
            return _mediator.Send(command);
        }

        public Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            // TODO this might not be the best way to do it
            // see this - https://www.youtube.com/watch?v=bda13k0vfc0
            return _mediator.Send(command);
        }

        public void Publish(IDomainEvent domainEvent)
        {
            _mediator.Publish(domainEvent);
        }

        public Task<TResult> Query<TResult>(IQuery<TResult> query)
        {
            return _mediator.Send(query);
        }
    }
}