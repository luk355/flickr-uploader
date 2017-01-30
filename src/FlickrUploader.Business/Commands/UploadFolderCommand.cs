using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class UploadFolderCommand : ICommand
    {
        public string Path { get; set; }
    }
}