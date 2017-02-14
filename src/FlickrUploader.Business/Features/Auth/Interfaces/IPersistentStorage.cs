namespace FlickrUploader.Business.Features.Auth
{
    public interface IPersistentStorage
    {
        void PersistValue(string key, string value);
        string LoadValue(string key);
    }
}