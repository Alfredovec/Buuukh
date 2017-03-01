using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Buh.Integration.Google.Models;
using Buh.Shared.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Buh.Integration.Google
{
    public class GoogleClient
    {
        private const string GoogleApiEndpointUrl = "https://www.googleapis.com/customsearch/v1";
        private const string ApiKey = "AIzaSyAqduwphG2hbWtO_vEMLSpYNrmks3iDxsA";

        public async Task<IEnumerable<GoogleImage>> SearchImagesAsync(string query, FileType filetype, int start = 1, int number = 10)
        {
            var searchType = "image";
            var imageSize = "large";
            var cx = "004187157729620767988:4oo0iyh_6is";
            var parameters = new Dictionary<string, string>
            {
                ["q"] = query,
                ["num"] = number.ToString(),
                ["start"] = start.ToString(),
                ["imgSize"] = imageSize,
                ["searchType"] = searchType,
                ["filetype"] = FileType.Jpg.ToString(),
                ["key"] = ApiKey,
                ["cx"] = cx
            };

            var result = await Search(parameters);
            var itemsJson = result.items.ToString();

            var googleImages = JsonConvert.DeserializeObject<IEnumerable<GoogleImage>>(itemsJson, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            return googleImages;
        }

        private async Task<dynamic> Search(IDictionary<string, string> parameters)
        {
            if (!parameters.ContainsKey("key"))
            {
                throw new ArgumentException("Please provide a google api key", nameof(parameters));
            }

            using (var httpClient = new HttpClient())
            {
                var parametersPath = 
                    parameters
                        .Aggregate("?", (s, pair) => s + $"{pair.Key}={pair.Value}&")
                        .Trim('&');

                var requestUrl = GoogleApiEndpointUrl + parametersPath;
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();

                dynamic responseObject = JsonConvert.DeserializeObject(responseJson);

                return responseObject;
            }
        }

        public GoogleImage GetRandomImage(string query, FileType type)
        {
            var random = new Random();
            var randomStart = random.Next(100);

            var googleImages = SearchImagesAsync(query, type, randomStart, number: 1).Result;

            var singleImage = googleImages.Single();

            return singleImage;
        }
    }
}