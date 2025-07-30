using System.Collections.Generic;
using Newtonsoft.Json;

namespace active_directory_wpf_msgraph_v2.GraphContacts
{
    public class ContactsResponse
    {
        [JsonProperty("@odata.context")] 
        public string Context { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }

        [JsonProperty("value")]
        public Dictionary<string, dynamic>[] Value { get; set; }
    }
}
