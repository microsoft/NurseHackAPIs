using HackAPIs.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public async Task<string> Send(MailChimp mailChimp)
        {

            HttpClient client = new HttpClient();
            client = new HttpClient();

            String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(mailChimp.User + ":" + mailChimp.Key));
            client.DefaultRequestHeaders.Add("Authorization", authenticationType.Basic + " " + authHeaer);

            HttpResponseMessage res = await client.GetAsync(mailChimp.URL);

            //        HttpResponseMessage res = client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result;
            client.Dispose();
            string response = await res.Content.ReadAsStringAsync();

            //     HttpStatusCode statusCode = res.StatusCode;
            return response;
        }
    }
}
