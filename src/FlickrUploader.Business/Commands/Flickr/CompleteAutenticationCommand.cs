using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands.Flickr
{
    public class CompleteAutenticationCommand : ICommand
    {
        public string Code { get; set; }

        public CompleteAutenticationCommand(string code)
        {
            Code = code;
        }
    }
}