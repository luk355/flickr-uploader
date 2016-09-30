using FlickrNet;

namespace FlickrUploader.Business
{
    public interface IFlickrClient
    {
        void SendAuthenticationRequest();

        void CompleteAutentication(string code);


        string UploadPicture(string path, string title, bool isPublic = false, bool isFamily = false,
            bool isFriend = false);

        string CreatePhotoset(string photoName, string primaryPhotoId);

        void AddPhotoToPhotoset(string photoId, string photosetId);

        PhotosetCollection PhotosetsGetList();
    }
}