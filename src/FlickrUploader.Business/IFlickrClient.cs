using FlickrNet;

namespace FlickrUploader.Business
{
    public interface IFlickrClient
    {
        void SendAuthenticationRequest();

        void CompleteAutentication(string code);

        string UploadPicture(string fileName, string title);

        PhotosetCollection PhotosetsGetList();
    }
}