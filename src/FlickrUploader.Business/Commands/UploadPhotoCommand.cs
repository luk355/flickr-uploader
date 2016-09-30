using System.Security.Policy;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class UploadPhotoCommand : ICommand
    {
        public string PhotosetName { get; set; }
        public string Path { get; set; }
    }
}