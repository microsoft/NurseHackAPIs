using HackAPIs.ViewModel.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class MailChimpService
    {
        public enum authenticationType
        {
            Basic,
            NTLM
        }

        //   public bool HttpPost(string body, out HttpStatusCode statusCode, out string response)
        public async Task<string> MemberSubcribe(MailChimp mailChimp)
        {

            HttpClient client = new HttpClient();
            client = new HttpClient();

            String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(mailChimp.User + ":" + mailChimp.Key));
            client.DefaultRequestHeaders.Add("Authorization", authenticationType.Basic + " " + authHeaer);

        //    HttpResponseMessage res = await client.GetAsync(mailChimp.URL);

            string payload = JsonConvert.SerializeObject(mailChimp.MailChimpPayload);

            HttpResponseMessage res = client.PostAsync(mailChimp.URL, new StringContent(payload, Encoding.UTF8, "application/json")).Result;
            client.Dispose();
            string response = await res.Content.ReadAsStringAsync();

            //     HttpStatusCode statusCode = res.StatusCode;
            return response;
        }
    }
}
