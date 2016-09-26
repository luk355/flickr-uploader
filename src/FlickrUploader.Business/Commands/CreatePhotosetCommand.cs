using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands
{
    public class CreatePhotosetCommand : ICommand<string>
    {
        public string Name { get; set; }

        public CreatePhotosetCommand(string name)
        {
            Name = name;
        }
    }
}