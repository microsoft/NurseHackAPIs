using HackAPIs.ViewModel.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class MailChimpService
    {

        private readonly HttpClient _client;
        private readonly MailChimpOptions _config;

        public MailChimpService(IOptions<MailChimpOptions> options, HttpClient client)
        {
            _config = options.Value;

            var authHeader = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_config.User + ":" + _config.Key));

            client.BaseAddress = new Uri(_config.Url);
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");
            _client = client;
        }
        public async Task<JObject> GetMembers()
        {
            var reqUrl = "lists/" + _config.Audience + "/members";

            HttpResponseMessage res = await _client.GetAsync(reqUrl);

            string json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(json) as JObject;
        }

        public async Task<string> AddMemberToList(string email, string displayName, MemberStatus memberStatus)
        {
            var payload = GetBodyContent(email, displayName, memberStatus);
            var reqUrl = "lists/" + _config.Audience + "/members";

            HttpResponseMessage res = await _client.PostAsync(reqUrl, payload);

            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject(json) as JObject;
                return respObj["id"].ToString();
            }

            return res.ReasonPhrase;
        }

        public async Task<string> UpdateMemberInList(string email, string displayName, string mailChimpId, MemberStatus memberStatus)
        {
            var payload = GetBodyContent(email, displayName, memberStatus);
            var reqUrl = "lists/" + _config.Audience + "/members/" + mailChimpId;

            HttpResponseMessage res = await _client.PutAsync(reqUrl, payload);
            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject(json) as JObject;
                return respObj["id"].ToString();
            }

            return res.ReasonPhrase;
        }

        public async Task<string> AddOrUpdateMember(string email, string displayName, MemberStatus status)
        {
            var mailChimpId = GetMailChimpId(email);
            var payload = GetBodyContent(email, displayName, status);

            var reqUrl = "lists/" + _config.Audience + "/members/" + mailChimpId;

            var res = await _client.PutAsync(reqUrl, payload);
            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject(json) as JObject;
                return respObj["id"].ToString();
            }

            return res.ReasonPhrase;
        }


        public async Task<bool> AddMemberTag(string mailChimpId, string tag)
        {
            var reqUrl = "lists/" + _config.Audience + "/members/" + mailChimpId + "/tags";
            var payload = new
            {
                tags = new[]
                {
                    new Tag
                    {
                        Name = tag,
                        Status = "active"
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _client.PostAsync(reqUrl, content);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public StringContent GetBodyContent(string email, string displayName, MemberStatus status)
        {
            var payload = new MailChimp
            {
                Email = email,
                FullName = displayName,
                Status = status,
                MergeFields = new Fields
                {
                    FNAME = displayName,
                    LNAME = displayName
                }
            };

            return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }

        private string GetMailChimpId(string email)
        {
            string result;
            using (var md5 = MD5.Create())
            {
                var input = Encoding.ASCII.GetBytes(email);
                result = string.Concat(md5.ComputeHash(input).Select(x => x.ToString("X2")));
            }
            return result.ToLower();
        }
    }
}
