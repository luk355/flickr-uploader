using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using FlickrUploader.Business.Commands.Flickr;
using Serilog;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Console
{
    public class Application
    {
        private readonly IUnifiedMediator<string> _mediator;

        public Application(IUnifiedMediator<string> mediator)
        {
            _mediator = mediator;
        }

        public Task Run()
        {
            return Task.Factory.StartNew(() =>
            {
                _mediator.Execute(new SendAuthenticationRequestCommand());
                System.Console.WriteLine("Please provide authentication code: ");
                string code = System.Console.ReadLine();

                _mediator.Execute(new CompleteAutenticationCommand(code.Replace("-", string.Empty)));

                _mediator.Execute(new UploadFolderCommand() {Path = ApplicationSettings.PhotoPath});

                Log.Information("All done! Pres any key to end the application :)");
                System.Console.ReadKey();
            });
        }
    }
}