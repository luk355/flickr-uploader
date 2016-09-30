using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using FlickrUploader.Business.DomainEvents;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class PhotoAggregate : ICommandHandler<UploadPhotoCommand>, IAsyncCommandHandler<UploadPhotosFromFolderCommand>
    {
        private readonly IFlickrClient _flickrClient;
        private readonly IFileSystem _fileSystem;
        private readonly IUnifiedMediator<string> _mediator;

        public PhotoAggregate(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator<string> mediator)
        {
            _flickrClient = flickrClient;
            _fileSystem = fileSystem;
            _mediator = mediator;
        }

        public Unit Handle(UploadPhotoCommand message)
        {
            // upload photo
            var id = _flickrClient.UploadPicture(message.Path, Path.GetFileName(message.Path));

            // add photo to photoset
            _mediator.Execute(new AddPhotoToPhotosetCommand()
            {
                PhotoId = id,
                PhotosetName = message.PhotosetName
            });

            _mediator.PublishAsync(new PhotoUploadedEvent() { Id = id, PhotoSet = message.PhotosetName });

            return Unit.Value;
        }

        public Task<Unit> Handle(UploadPhotosFromFolderCommand message)
        {
            return Task<Unit>.Factory.StartNew(() =>
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(message.FolderPath);

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", message.FolderPath);
                    return Unit.Value;
                }

                var photos = dirInfo.EnumerateFiles("*.jpg").ToList();

                if (!photos.Any())
                {
                    Log.Information("No photos found in {PhotoFolder}", message.FolderPath);
                    return Unit.Value;
                }

                // get photoSet
                var photosetName = dirInfo.Name;

                // upload photos one by one

                Log.Information("Uploading {Photos} photos located in {Folder} folder.", photos.Select(x => x.Name), message.FolderPath);
                foreach (var photo in photos)
                {
                    _mediator.Execute(new UploadPhotoCommand() { Path = photo.FullName, PhotosetName = photosetName });
                }

                _mediator.Publish(new PhotosFromFolderUploadedEvent() { Id = message.FolderPath });

                return Unit.Value;
            });
        }


    }
}
