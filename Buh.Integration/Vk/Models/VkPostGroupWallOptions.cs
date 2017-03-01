using System;
using System.Collections.Generic;
using VkNet.Model.Attachments;

namespace Buh.Integration.Vk.Models
{
    public class VkPostGroupWallOptions
    {
        public int GroupId { get; set; }

        public string Message { get; set; }

        public DateTime PublishDate { get; set; }

        public IEnumerable<MediaAttachment> MediaAttachments { get; set; }
    }
}