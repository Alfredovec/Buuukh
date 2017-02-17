using System;
using System.Linq;

namespace Buh.Scheduler.Jobs
{
    internal class BuhJob : VkJob
    {
        protected const int GroupId = -134042408;
        protected const string SearchQuery = "raccoon";
        protected const FileType Type = FileType.Jpg;

        private readonly BuhService _buhService;
        private readonly GoogleSearchService _googleSearch;

        public BuhJob()
        {
            _buhService = new BuhService();
            _googleSearch = new GoogleSearchService();
        }

        protected override void Execute()
        {
            var random = new Random();
            var randomHours = random.Next(24);
            var randomMinutes = random.Next(60);

            var googleImages = _googleSearch.SearchImagesAsync(SearchQuery, Type).Result;

            var message = _buhService.Generate();
            var publishDate = DateTime.Now.AddHours(randomHours).AddMinutes(randomMinutes);
            var racoonImage = Enumerable.ElementAt(googleImages, 3).Link;
            var photo = VkService.GetPhoto(racoonImage, Math.Abs(GroupId));
            var attachments = new List<MediaAttachment>
            {
                new Photo
                {
                    Id = photo.Id,
                    OwnerId = photo.OwnerId.Value,
                    AlbumId = photo.AlbumId.Value,
                    PhotoSrc = photo.PhotoSrc
                }
            };
            
            VkService.PostGroupWall(GroupId, message, publishDate, attachments);
        }
    }
}
