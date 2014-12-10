using System;
using Newtonsoft.Json;
using Photolane.Shared.Helper;

namespace Photolane.Shared.Model
{
    public class Photo
    {
        public Photo()
        {
            ResourceName = Guid.NewGuid().ToString();
            ContainerName = "photolane";
            PhotoTimeStamp = DateTime.Now;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string ProfilePicture
        {
            get
            {
                if (UserId != null)
                {
                    string[] splitResult = UserId.Split(':');
                    return "http://graph.facebook.com/" + splitResult[1] + "/picture";
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "photoTimestamp")]
        public DateTime PhotoTimeStamp { get; set; }

        public string RelativePhotoTimeStamp
        {
            get { return RelativeTimeConverter.ConvertToRelativeTime(PhotoTimeStamp); }
        }

        [JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }

        [JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty(PropertyName = "sasQueryString")]
        public string SasQueryString { get; set; }
    }
}