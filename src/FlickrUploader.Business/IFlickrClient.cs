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

        (string token, string secret) GetAccessToken();

        void SetAccessToken(string token, string secret);

        void ResetAccessData();

        bool IsAccessTokenValid();

        PhotosetCollection PhotosetsGetList();
    }
}