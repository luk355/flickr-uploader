using System.IO;
using System.IO.Abstractions;
using MediatR;
using FlickrUploader.Core.Eventing;
using FlickrUploader.Core.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrUploader.Business.Commands
{
    public static class UploadPhoto
    {

        public class Command : ICommand
        {
            public string Path { get; set; }
            public string PhotosetName { get; set; }
        }

        public class PhotoAggregate : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IFileSystem _fileSystem;
            private readonly IUnifiedMediator _mediator;

            public PhotoAggregate(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator mediator)
            {
                _flickrClient = flickrClient;
                _fileSystem = fileSystem;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // upload photo
                var id = _flickrClient.UploadPicture(request.Path, Path.GetFileNameWithoutExtension(request.Path));

                // add photo to photoset
                await _mediator.Execute(new AddPhotoToPhotoset.Command()
                {
                    PhotoId = id,
                    PhotosetName = request.PhotosetName
                });

                _mediator.Publish(new PhotoUploadedEvent() { Id = id, Path = request.Path, PhotoSet = request.PhotosetName });

                return Unit.Value;
            }
        }

        public class PhotoUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }

            public string Path { get; set; }

            public string PhotoSet { get; set; }
        }
    }
}