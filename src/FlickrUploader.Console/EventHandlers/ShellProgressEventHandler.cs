using FlickrUploader.Console.ShellProgress;
using FlickrUploader.Core.Eventing;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using static FlickrUploader.Business.Commands.UploadPhoto;
using static FlickrUploader.Business.Commands.UploadPhotosFromFolder;

namespace FlickrUploader.Console.EventHandlers
{
    public class ShellProgressEventHandler : 
        IAsyncDomainEventHandler<UploadOfPhotosStarted>,
        IDomainEventHandler<PhotoUploadedEvent>,
        IDomainEventHandler<PhotosFromFolderUploadedEvent>
    {
        public ShellProgressEventHandler(ShellProgressBarManager progressBarManager, IFileSystem fileSystem)
        {
            _progressBarManager = progressBarManager;
            _fileSystem = fileSystem;
        }

        private readonly ShellProgressBarManager _progressBarManager;
        private readonly IFileSystem _fileSystem;

        public Task Handle(UploadOfPhotosStarted notification, CancellationToken cancellationToken)
        {
            var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(notification.Id);

            _progressBarManager.StartNewProgressBar(dirInfo.Name, notification.PhotosToUpload);

            return Task.CompletedTask;
        }

        public Task Handle(PhotoUploadedEvent notification, CancellationToken cancellationToken)
        {
            var fileInfo = _fileSystem.FileInfo.FromFileName(notification.Path);

            _progressBarManager.RecordProgress(fileInfo.Directory.Name, fileInfo.Name);

            return Task.CompletedTask;
        }

        public Task Handle(PhotosFromFolderUploadedEvent notification, CancellationToken cancellationToken)
        {
            var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(notification.Id);

            _progressBarManager.RemoveProgressBar(dirInfo.Name);
            return Task.CompletedTask;
        }
    }
}
