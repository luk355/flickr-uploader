using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class UploadFolderCommand : IAsyncCommand
    {
        public string Path { get; set; }
    }
}