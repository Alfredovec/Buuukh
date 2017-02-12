using System;
using System.Collections.Generic;
using System.Linq;
using Buh.ConsoleApp.Google;
using ConsoleApplication1;
using Quartz;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Buh.ConsoleApp
{
    internal class BuhJob : VkJob
    {
        protected const int GroupId = -134042408;
        protected const string SearchQuery = "Racoon";
        protected const FileType Type = FileType.Jpg;

        private readonly BuhService _buhService;
        private readonly GoogleSearchService _googleSearch;

        public BuhJob(BuhService buhService, GoogleSearchService googleSearch)
        {
            _buhService = buhService;
            _googleSearch = googleSearch;
        }

        protected override void Execute()
        {
            var random = new Random();
            var randomHours = random.Next(24);
            var randomMinutes = random.Next(60);
            
            var message = _buhService.Generate();
            var publishDate = DateTime.Now.AddHours(randomHours).AddMinutes(randomMinutes);
            var images = _googleSearch.SearchImagesAsync(SearchQuery, Type).Result;
            var racoonImage = images.First().Link;

            Vk.Wall.Post(new WallPostParams
            {
                OwnerId = GroupId,
                Message = message,
                FromGroup = true,
                PublishDate = publishDate,
                Attachments = new List<MediaAttachment>
                {
                    new Photo
                    {
                        PhotoSrc = new Uri(racoonImage)
                    }
                }
            });
        }
    }
}
