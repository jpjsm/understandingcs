using System;
using Newtonsoft.Json;

namespace active_directory_wpf_msgraph_v2.GraphContacts
{
    public class GraphContactCatergory
    {
        [JsonProperty("@odata.etag")]
        public string Etag { get; set; }
    }
}
