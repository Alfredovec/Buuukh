using System;
using System.Collections.Generic;
using System.Linq;
using Buh.Domain.Services;
using Buh.Integration.Google;
using Buh.Integration.Vk.Models;
using Buh.Shared.Enums;
using VkNet.Model.Attachments;

namespace Buh.Scheduler.Jobs
{
    public class BuhJob : VkJob
    {
        protected const int GroupId = -134042408;
        protected const string SearchQuery = "raccoon";
        protected const FileType Type = FileType.Jpg;

        private readonly BuhService _buhService;
        private readonly GoogleClient _googleClient;

        public BuhJob()
        {
            _buhService = new BuhService();
            _googleClient = new GoogleClient();
        }

        protected override void Execute()
        {
            var random = new Random();
            var randomHours = random.Next(24);
            var randomMinutes = random.Next(60);

            var googleImages = _googleClient.SearchImagesAsync(SearchQuery, Type).Result;

            var message = _buhService.Generate();
            var publishDate = DateTime.Now.AddHours(randomHours).AddMinutes(randomMinutes);
            var racoonImage = googleImages.ElementAt(3).Link;
            var photo = VkClient.GetPhoto(racoonImage, Math.Abs(GroupId));
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
            
            var postOptions = new VkPostGroupWallOptions
            {
                GroupId = GroupId,
                Message = message,
                PublishDate = publishDate,
                MediaAttachments = attachments
            };

            VkClient.PostGroupWall(postOptions);
        }
    }
}
