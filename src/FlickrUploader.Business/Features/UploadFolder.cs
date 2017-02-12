using FlickrUploader.Business.Extensions;
using FlickrUploader.Core.Eventing;
using MediatR;
using Serilog;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using UnifiedMediatR.Mediator;

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
            private readonly IUnifiedMediator<string> _mediator;

            private const string ProcessedFileName = ".processed";

            public CommandHandler(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator<string> mediator)
            {
                _flickrClient = flickrClient;
                _fileSystem = fileSystem;
                _mediator = mediator;
            }

            Unit IRequestHandler<Command, Unit>.Handle(Command message)
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(message.Path);

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", message.Path);
                    return Unit.Value;
                }

                if (!dirInfo.HasWritePermisssion())
                {
                    Log.Error("Missing write rights to folder {FolderName}!", message.Path);
                    return Unit.Value;
                }

                // upload photos from all sub-folders
                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    _mediator.Execute(new Command() { Path = directory.FullName });
                }

                if (dirInfo.EnumerateFiles(ProcessedFileName).Any())
                {
                    Log.Information("Folder {FolderName} has been already processed. Skipping...", message.Path);
                    return Unit.Value;
                }

                // Upload all photos in folder
                _mediator.Execute(new UploadPhotosFromFolder.Command() { FolderPath = message.Path });

                // creates processed file in folder
                var processedFile = new FileInfo(Path.Combine(dirInfo.FullName, ProcessedFileName));
                processedFile.Create().Close();

                _mediator.Publish(new FolderUploadedEvent() { Id = message.Path });
                Log.Information("Upload of all photos within folder {FolderPath} is finished!", message.Path);

                return Unit.Value;
            }
        }

        public class FolderUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }
    }

    
}