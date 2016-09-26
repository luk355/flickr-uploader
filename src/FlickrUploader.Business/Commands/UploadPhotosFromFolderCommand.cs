using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class UploadPhotosFromFolderCommand : IAsyncCommand
    {
        public string FolderPath { get; set; }
    }
}