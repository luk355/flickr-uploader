using System.Threading;
using System.Threading.Tasks;
using FlickrUploader.Core.Eventing;
using FlickrUploader.Core.Mediator;

namespace FlickrUploader.Business.Commands
{
    public static class CreatePhotoset
    {
        public class Command : ICommand<string>
        {
            public string Name { get; set; }

            public string PrimaryPhotoId { get; set; }

            public Command(string name, string primaryPhotoId)
            {
                PrimaryPhotoId = primaryPhotoId;
                Name = name;
            }
        }

        public class CommandHandler : ICommandHandler<Command, string>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IUnifiedMediator _mediator;

            public CommandHandler(IFlickrClient flickrClient, IUnifiedMediator mediator)
            {
                _flickrClient = flickrClient;
                _mediator = mediator;
            }

            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_flickrClient.CreatePhotoset(request.Name, request.PrimaryPhotoId));
            }
        }

        public class PhotosetCreatedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }
    }
}