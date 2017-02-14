namespace FlickrUploader.Business.Features.Auth
{
    public interface IAuthCodeProvider
    {
        string GetCode();
    }
}
