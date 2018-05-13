using System.IO;
using System.Linq;
using FlickrNet;
using FlickrUploader.Business.Extensions;
using System.Collections.Generic;
using FlickrUploader.Business.Utils;
using System;

namespace FlickrUploader.Business
{
    public class FlickrClient : IFlickrClient
    {
        private readonly Flickr _flickr;

        private OAuthRequestToken requestToken;

        public FlickrClient(string apiKey, string secret)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException("Token or Secret value has not been set. Please check appsettings.json and update values first.");
            }

            _flickr = new Flickr(apiKey, secret);
        }

        public void SendAuthenticationRequest()
        {
            requestToken = _flickr.OAuthGetRequestToken("oob");

            string url = _flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);

            UrlUtils.OpenBrowser(url);
        }

        public void CompleteAutentication(string code)
        {
            _flickr.OAuthGetAccessToken(requestToken, code);
        }

        public string UploadPicture(string path, string title, bool isPublic = false, bool isFamily = false, bool isFriend = false)
        {
            string fileName = Path.GetFileName(title);

            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var keywords = ImageUtils.GetKeywords(stream);
                keywords.Add("AutoUploaded");

                string str = _flickr.UploadPicture(stream, fileName, title, "", keywords.Aggregate(string.Empty, (e, r) => $"{r},{e}"), false, false, false, ContentType.None, SafetyLevel.None, HiddenFromSearch.None);
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

        (string token, string secret) IFlickrClient.GetAccessToken()
        {
            return (_flickr.OAuthAccessToken, _flickr.OAuthAccessTokenSecret);
        }

        public void SetAccessToken(string token, string secret)
        {
            _flickr.OAuthAccessToken = token;
            _flickr.OAuthAccessTokenSecret = secret;
        }

        public bool IsAccessTokenValid()
        {
            try
            {
                var auth = _flickr.AuthOAuthCheckToken();

                if (auth == null || auth.User == null)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void ResetAccessData()
        {
            _flickr.OAuthAccessToken = null;
            _flickr.OAuthAccessTokenSecret = null;
        }

        public IList<string> GetPhotoNamesInPhotoset(string photosetId)
        {
            return _flickr.PhotosetsGetPhotos(photosetId).Select(x => x.Title).ToList();
        }
    }
}