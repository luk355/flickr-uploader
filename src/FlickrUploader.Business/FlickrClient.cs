using System;
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