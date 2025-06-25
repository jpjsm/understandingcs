using System;
using active_directory_wpf_msgraph_v2.GraphContacts;
using Newtonsoft.Json;

namespace active_directory_wpf_msgraph_v2.GraphContacts
{
    public class GraphContact
    {
        [JsonProperty("@odata.etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("categories")]
        public GraphContactCatergory[] Categories { get; set; }
    }
}
