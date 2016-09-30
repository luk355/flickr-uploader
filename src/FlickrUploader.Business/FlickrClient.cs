using System;
using System.IO;
using System.Linq;
using FlickrNet;

namespace FlickrUploader.Business
{
    public class FlickrClient : IFlickrClient
    {
        private readonly Flickr _flickr;

        private OAuthRequestToken requestToken;

        public FlickrClient(string apiKey, string secret)
        {
            _flickr = new Flickr(apiKey, secret);
           
        }

        public void SendAuthenticationRequest()
        {
            requestToken = _flickr.OAuthGetRequestToken("oob");

            string url = _flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);

            System.Diagnostics.Process.Start(url);
        }

        public void CompleteAutentication(string code)
        {
            _flickr.OAuthGetAccessToken(requestToken, code);
        }

        public string UploadPicture(string path, string title, bool isPublic = false, bool isFamily = false, bool isFriend = false)
        {
            string fileName1 = Path.GetFileName(title);
            using (Stream stream = (Stream)new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string str = _flickr.UploadPicture(stream, fileName1, title, "", "AutoUploaded", false, false, false, ContentType.None, SafetyLevel.None, HiddenFromSearch.None);
                stream.Close();
                return str;
            }
        }

        public string CreatePhotoset(string photoName, string primaryPhotoId)
        {
            return _flickr.PhotosetsCreate(photoName, primaryPhotoId)?.PhotosetId;
        }

        public void AddPhotoToPhotoset(string photoId, string photosetId)
        {
            _flickr.PhotosetsAddPhoto(photosetId, photoId);
        }

        public PhotosetCollection PhotosetsGetList()
        {
            return _flickr.PhotosetsGetList();
        }
    }
}