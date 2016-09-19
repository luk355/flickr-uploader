using System;
using System.Linq;
using FlickrNet;

namespace FlickrUploader.Business
{
    public class FlickrClient : IFlickrClient
    {
        private readonly Flickr _flickr;

        public FlickrClient(string apiKey)
        {
            _flickr = new Flickr(apiKey);
        }

        public string UploadPicture(string fileName, string title)
        {
            throw new NotImplementedException();
        }

        public PhotosetCollection PhotosetsGetList()
        {
            return _flickr.PhotosetsGetList();
        }
    }
}