using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HackAPIs.ViewModel.Util
{
    public class MailChimp
    {
        [JsonProperty("email_address")]
        public string Email { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("status")]
        public MemberStatus Status { get; set; }
        [JsonProperty("merge_fields")]
        public Fields MergeFields { get; set; }

    }

    public class Fields
    {
        public string FNAME { get; set; }
        public string LNAME { get; set; }
    }

    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MemberStatus
    {
        subscribed,
        unsubscribed
    }
}
