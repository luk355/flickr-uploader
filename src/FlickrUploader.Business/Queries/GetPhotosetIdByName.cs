using System;
using System.Linq;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Queries
{
    public class GetPhotosetIdByName : IQuery<string>
    {
        public GetPhotosetIdByName(string photosetName)
        {
            PhotosetName = photosetName;
        }

        public string PhotosetName { get; }
    }

    public class GetPhotosetIdByNameHandler : IQueryHandler<GetPhotosetIdByName, string>
    {
        private readonly IFlickrClient _flickrClient;

        public GetPhotosetIdByNameHandler(IFlickrClient flickrClient)
        {
            _flickrClient = flickrClient;
        }

        public string Handle(GetPhotosetIdByName message)
        {
            return _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == message.PhotosetName)?.Title;
        }
    }
}
