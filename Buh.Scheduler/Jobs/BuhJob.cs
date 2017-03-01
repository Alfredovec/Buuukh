using System;
using System.Collections.Generic;
using System.Linq;
using Buh.Domain.Services;
using Buh.Integration.Google;
using Buh.Integration.Google.Models;
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
        protected static string[] KnownExtensions = { "jpg", "jpeg", "png" };

        private readonly BuhService _buhService;
        private readonly GoogleClient _googleClient;

        public BuhJob()
        {
            _buhService = new BuhService();
            _googleClient = new GoogleClient();
        }

        protected override void Execute()
        {
            var message = _buhService.GenerateRandomMessage();
            var publishDate = GenerateRandomDateTime(DateTime.Now, DateTime.Now.AddDays(1));

            GoogleImage racoonImage;
            do
            {
                racoonImage = _googleClient.GetRandomImage(SearchQuery, Type);
            } while (KnownExtensions.Any(knownExtension => racoonImage.Link.EndsWith("." + knownExtension, StringComparison.InvariantCultureIgnoreCase)));
            
            var positiveGroupId = Math.Abs(GroupId);
            var photo = VkClient.GetPhoto(racoonImage.Link, positiveGroupId);
            var attachments = new List<MediaAttachment>
            {
                new Photo
                {
                    Id = photo.Id,
                    OwnerId = photo.OwnerId ?? -1,
                    AlbumId = photo.AlbumId ?? -1,
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

        private DateTime GenerateRandomDateTime(DateTime lowerBound, DateTime upperBound)
        {
            var interval = upperBound - lowerBound;
            var totalSeconds = (int)Math.Floor(interval.TotalSeconds);

            var random = new Random();
            var randomSeconds = random.Next(totalSeconds);

            var publishDate = lowerBound.AddSeconds(randomSeconds);

            return publishDate;
        }
    }
}
