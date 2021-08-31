using HackAPIs.ViewModel.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
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

        public async Task<string> AddMemberToList(string email, string fName, string lName, string mailChimpId, string memberStatus)
        {
            var payload = GetBodyContent(email, fName, lName, mailChimpId, memberStatus);
            var reqUrl = "lists/" + _config.Audience + "/members";

            HttpResponseMessage res = await _client.PostAsync(reqUrl, payload);

            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject(json) as JObject;
                return respObj["id"].ToString();
            }

            return res.StatusCode.ToString();
        }

        public async Task<string> UpdateMemberInList(string email, string fName, string lName, string mailChimpId, string memberStatus)
        {
            var payload = GetBodyContent(email, fName, lName, mailChimpId, memberStatus);
            var reqUrl = "lists/" + _config.Audience + "/members/"+ mailChimpId;

            HttpResponseMessage res = await _client.PutAsync(reqUrl, payload);
            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject(json) as JObject;
                return respObj["id"].ToString();
            }

            return res.StatusCode.ToString();
        }

        public async Task<JObject> GetMembers()
        {
            var reqUrl = "lists/" + _config.Audience + "/members";

            HttpResponseMessage res = await _client.GetAsync(reqUrl);
           
            string json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(json) as JObject;
        }

        public StringContent GetBodyContent(string email, string FName, string LName, string mailChimpId, string memberStatus)
        {
            var mc = new MailChimp
            {
                UserID = mailChimpId,
                mailChimpPayload = new MailChimpPayload
                {
                    email_address = email,
                    status = memberStatus,
                    merge_fields = new Fields
                    {
                        FNAME = FName,
                        LNAME = LName
                    }
                }
            };

            return new StringContent(JsonConvert.SerializeObject(mc), Encoding.UTF8, "application/json");
        }
    }
}
