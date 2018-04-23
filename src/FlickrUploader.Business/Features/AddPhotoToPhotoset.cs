using MediatR;
using System.Linq;
using FlickrUploader.Business.Exceptions;
using FlickrUploader.Core.Eventing;
using FlickrUploader.Core.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrUploader.Business.Commands
{
    public static class AddPhotoToPhotoset
    {
        public class Command : ICommand
        {
            public string PhotoId { get; set; }
            public string PhotosetName { get; set; }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IUnifiedMediator _mediator;

            public CommandHandler(IFlickrClient flickrClient, IUnifiedMediator mediator)
            {
                _flickrClient = flickrClient;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var photosetId =
                _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == request.PhotosetName)?.PhotosetId;

                // create photoset if not exists
                // this adds already photoToPhotoset -> no additional photo addtion needed
                if (string.IsNullOrEmpty(photosetId))
                {
                    photosetId = await _mediator.Execute(new CreatePhotoset.Command(request.PhotosetName, request.PhotoId));

                    _mediator.Publish(new PhotoAddedAsMainToPhotosetEvent() { Id = request.PhotoId, PhotosetId = photosetId });

                    return Unit.Value;
                }

                if (string.IsNullOrEmpty(photosetId))
                {
                    throw new UnableToCreatePhotoSetException($"Unable to retreive {request.PhotosetName} photoset.");
                }

                _flickrClient.AddPhotoToPhotoset(request.PhotoId, photosetId);

                _mediator.Publish(new PhotoAddedToPhotosetEvent() { Id = request.PhotoId, PhotosetId = photosetId });

                return Unit.Value;
            }
        }

        public class PhotoAddedAsMainToPhotosetEvent : PhotoAddedToPhotosetEvent
        {
        }

        public class PhotoAddedToPhotosetEvent : IDomainEvent
        {
            public string Id { get; set; }

            public string PhotosetId { get; set; }
        }
    }
}