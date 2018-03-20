using FlickrUploader.Business.Extensions;
using FlickrUploader.Core.Eventing;
using FlickrUploader.Core.Mediator;
using MediatR;
using Serilog;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace FlickrUploader.Business.Commands
{
    public static class UploadFolder
    {
        public class Command : ICommand
        {
            public string Path { get; set; }
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

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(request.Path);

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", request.Path);
                    return Task.FromResult(Unit.Value);
                }

                if (!dirInfo.HasWritePermisssion())
                {
                    Log.Error("Missing write rights to folder {FolderName}!", request.Path);
                    Task.FromResult(Unit.Value);
                }

                // upload photos from all sub-folders
                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    _mediator.Execute(new UploadFolder.Command() { Path = directory.FullName });
                }

                // Upload all photos in folder
                _mediator.Execute(new UploadPhotosFromFolder.Command() { FolderPath = request.Path });


                _mediator.Publish(new FolderUploadedEvent() { Id = request.Path });
                Log.Information("Upload of all photos within folder {FolderPath} is finished!", request.Path);

                return Task.FromResult(Unit.Value);
            }
        }

        public class FolderUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }
    }

    
}