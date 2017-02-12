using FlickrUploader.Core.Eventing;
using UnifiedMediatR.Mediator;

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
            private readonly IUnifiedMediator<string> _mediator;

            public CommandHandler(IFlickrClient flickrClient, IUnifiedMediator<string> mediator)
            {
                _flickrClient = flickrClient;
                _mediator = mediator;
            }

            public string Handle(Command message)
            {
                return _flickrClient.CreatePhotoset(message.Name, message.PrimaryPhotoId);
            }


        }

        public class PhotosetCreatedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }
    }
}