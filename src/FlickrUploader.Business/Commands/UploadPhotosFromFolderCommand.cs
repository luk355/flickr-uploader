using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class UploadPhotosFromFolderCommand : ICommand
    {
        public string FolderPath { get; set; }
    }
}