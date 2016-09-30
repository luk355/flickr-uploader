using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class AddPhotoToPhotosetCommand : ICommand
    {
        public string PhotoId { get; set; }
        public string PhotosetName { get; set; }
    }
}