﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Buh.ConsoleApp.Models;
using Newtonsoft.Json.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Buh.ConsoleApp.Services
{
    public class VkService
    {
        protected const ulong ApplicationId = 5749376;

        protected readonly VkApi Vk;

        public VkService()
        {
            Vk = new VkApi();
            Vk.OnTokenExpires += api => api.RefreshToken();
        }

        public Photo GetPhoto(string imageSrc, int groupId)
        {
            var uploadServer = GetWallUploadServer(groupId);

            var fileName = $@"racoon-{Guid.NewGuid().ToString().Split('-').First()}";
            var fileExtension = imageSrc.Split('.').Last().ToLower();
            var filePath = $@"C:/Data/Nowork/MyApps/Buh/images/{fileName}.{fileExtension}";

            var webClient = new WebClient();
            webClient.DownloadFile(new Uri(imageSrc), filePath);

            var obj = PhotosUploadPhotoToUrl(uploadServer.UploadUrl, filePath);

            var photos = Vk.Photo.SaveWallPhoto(obj.ToString(), groupId: (ulong)groupId);

            var photo = photos.Single();
            if (photo.Id != null) return photo;

            throw new Exception();
        }

        private JObject PhotosUploadPhotoToUrl(string url, string filePath)
        {
            WebClient myWebClient = new WebClient();
            byte[] responseArray = myWebClient.UploadFile(url, filePath);
            var json = JObject.Parse(Encoding.ASCII.GetString(responseArray));

            return json;
        }

        private UploadServer GetWallUploadServer(int groupId)
        {
            var uploadServerInfo = Vk.Photo.GetWallUploadServer(groupId);

            return new UploadServer
            {
                UploadUrl = uploadServerInfo.UploadUrl
            };
        }

        public void Authorize(string login, string password)
        {
            if (Vk.IsAuthorized == false)
            {
                Vk.Authorize(new ApiAuthParams
                {
                    ApplicationId = ApplicationId,
                    Login = login,
                    Password = password,
                    Settings = Settings.All
                });
            }
        }

        public void PostGroupWall(int groupId, string message, DateTime publishDate, List<MediaAttachment> attachments)
        {
            Vk.Wall.Post(new WallPostParams
            {
                OwnerId = groupId,
                Message = message,
                FromGroup = true,
                PublishDate = publishDate,
                Attachments = attachments
            });
        }
    }
}