using HackAPIs.ViewModel.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string ApiExt = "";
        public enum authenticationType
        {
            Basic,
            NTLM
        }

        public enum AudienceMemberType
        {
            New,
            Update,
            subscribed,
            unsubscribed
        }

        //   public bool HttpPost(string body, out HttpStatusCode statusCode, out string response)
        public async Task<JObject> AddMemberToList(MailChimp mailChimp)
        {
            JObject jsonObject = null;
            ApiExt = "lists/" + mailChimp.Audience + "/members";
            HttpClient client = new HttpClient();
            client = new HttpClient();

            String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(mailChimp.User + ":" + mailChimp.Key));
            client.DefaultRequestHeaders.Add("Authorization", authenticationType.Basic + " " + authHeaer);

        //    HttpResponseMessage res = await client.GetAsync(mailChimp.URL);

            string payload = JsonConvert.SerializeObject(mailChimp.mailChimpPayload);

            HttpResponseMessage res = client.PostAsync(mailChimp.URL+ApiExt, new StringContent(payload, Encoding.UTF8, "application/json")).Result;
            client.Dispose();
            string json = await res.Content.ReadAsStringAsync();
            
            jsonObject = JsonConvert.DeserializeObject(json) as JObject;

            //     HttpStatusCode statusCode = res.StatusCode;
            return jsonObject;
        }

        public async Task<JObject> UpdateMemberInList(MailChimp mailChimp)
        {
            JObject jsonObject = null;
            ApiExt = "lists/" + mailChimp.Audience + "/members/"+ "2a4ad92a10830b0872818a3a625550c6";
            HttpClient client = new HttpClient();
            client = new HttpClient();

            String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(mailChimp.User + ":" + mailChimp.Key));
            client.DefaultRequestHeaders.Add("Authorization", authenticationType.Basic + " " + authHeaer);

            //    HttpResponseMessage res = await client.GetAsync(mailChimp.URL);

            string payload = JsonConvert.SerializeObject(mailChimp.mailChimpPayload);

            HttpResponseMessage res = client.PutAsync(mailChimp.URL + ApiExt, new StringContent(payload, Encoding.UTF8, "application/json")).Result;
            client.Dispose();
            string json = await res.Content.ReadAsStringAsync();
            jsonObject = JsonConvert.DeserializeObject(json) as JObject;

            //     HttpStatusCode statusCode = res.StatusCode;
            return jsonObject;
        }
        public async Task<JObject> GetMembers(MailChimp mailChimp)
        {
            JObject jsonObject = null;

            ApiExt = "lists/" + mailChimp.Audience + "/members";
     //       ApiExt = "lists";
            HttpClient client = new HttpClient();
            client = new HttpClient();

            String authHeaer = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(mailChimp.User + ":" + mailChimp.Key));
            client.DefaultRequestHeaders.Add("Authorization", authenticationType.Basic + " " + authHeaer);

            HttpResponseMessage res = await client.GetAsync(mailChimp.URL + ApiExt);

           
            client.Dispose();
            string json = await res.Content.ReadAsStringAsync();
            jsonObject = JsonConvert.DeserializeObject(json) as JObject;

            //     HttpStatusCode statusCode = res.StatusCode;
            return jsonObject;
        }

        public async Task<string> InvokeMailChimp(string email, string FName, string LName, string memberStatus, int type)
        {
            string id = "";
            JObject jObject = null;
            try
            {
                MailChimpService mailChimpService = new MailChimpService();

                MailChimp mailChimp = new MailChimp
                {
                    Audience = UtilConst.MailChimpAudience,
                    Key = UtilConst.MailChimpKey,
                    URL = UtilConst.MailChimpURL,
                    User = UtilConst.MailChimpUser,
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

                if (type == 1)
                    jObject = await mailChimpService.AddMemberToList(mailChimp);
                else if (type == 2)
                    jObject = await mailChimpService.UpdateMemberInList(mailChimp);

                id = jObject["id"].ToString();



            }
            catch (Exception ex)
            {

            }

            return id;
        }
    }
}
