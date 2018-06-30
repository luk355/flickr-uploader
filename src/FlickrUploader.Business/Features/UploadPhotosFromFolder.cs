using System.IO.Abstractions;
using System.Linq;
using MediatR;
using Serilog;
using FlickrUploader.Core.Eventing;
using System.IO;
using FlickrUploader.Core.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrUploader.Business.Commands
{
    public static class UploadPhotosFromFolder
    {

        public class Command : ICommand
        {
            public string FolderPath { get; set; }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IFileSystem _fileSystem;
            private readonly IUnifiedMediator _mediator;

            public CommandHandler(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator mediator)
            {
                _flickrClient = flickrClient;
                _fileSystem = fileSystem;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(request.FolderPath);

                var photosetName = dirInfo.Name;

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", request.FolderPath);
                    return Unit.Value;
                }

                var photosToUpload = dirInfo.EnumerateFiles("*.jpg").ToList();

                if (!photosToUpload.Any())
                {
                    Log.Debug("No photos found in {PhotoFolder}", request.FolderPath);
                    return Unit.Value;
                }

                // remove any already already uploaded photos - checking by photo title at the moment
                var photosetId = _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == photosetName)?.PhotosetId;

                if (photosetId != null)
                {
                    var photosInPhotoset = _flickrClient.GetPhotoNamesInPhotoset(photosetId);
                    Log.Information("Album already exists in Flickr with {Photos}. These are not being uploaded again.", photosInPhotoset);

                    photosToUpload.RemoveAll(x => photosInPhotoset.Contains(Path.GetFileNameWithoutExtension(x.FullName)));

                    if (!photosToUpload.Any())
                    {
                        Log.Information("All photos from {PhotoFolder} has been uploaded already.", request.FolderPath);
                        return Unit.Value;
                    }
                }

                _mediator.Publish(new UploadOfPhotosStarted(request.FolderPath, photosToUpload.Count));

                // upload photos one by one
                Log.Information("Uploading {PhotoCount} photos located in {Folder} folder. {Photos}", photosToUpload.Count, request.FolderPath, photosToUpload.Select(x => x.Name));
                foreach (var photo in photosToUpload)
                {
                    await _mediator.Execute(new UploadPhoto.Command() { Path = photo.FullName, PhotosetName = photosetName });
                }

                _mediator.Publish(new PhotosFromFolderUploadedEvent() { Id = request.FolderPath });

                return Unit.Value;
            }
        }

        public class UploadOfPhotosStarted : IAsyncDomainEvent
        {
            public UploadOfPhotosStarted(string folderName, int photosToUpload)
            {
                Id = folderName;
                PhotosToUpload = photosToUpload;
            }

            public string Id { get; }

            public int PhotosToUpload { get; }
        }

        public class PhotosFromFolderUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }

    }
}