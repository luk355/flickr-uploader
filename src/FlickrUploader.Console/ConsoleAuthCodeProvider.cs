using FlickrUploader.Business.Features.Auth;

namespace FlickrUploader.Console
{
    class ConsoleAuthCodeProvider : IAuthCodeProvider
    {
        public string GetCode()
        {
            System.Console.WriteLine("Please provide authentication code: ");
            string code = System.Console.ReadLine();

            return code.Replace("-", string.Empty);
        }
    }
}
