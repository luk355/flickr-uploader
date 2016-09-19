using FlickrNet;

namespace FlickrUploader.Business
{
    public interface IFlickrClient
    {
        string UploadPicture(string fileName, string title);

        PhotosetCollection PhotosetsGetList();
    }
}