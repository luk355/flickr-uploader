using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class CreatePhotosetCommand : ICommand<string>
    {
        public string Name { get; set; }

        public string PrimaryPhotoId { get; set; }

        public CreatePhotosetCommand(string name, string primaryPhotoId)
        {
            PrimaryPhotoId = primaryPhotoId;
            Name = name;
        }
    }
}