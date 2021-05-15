using HackAPIs.ViewModel.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class GitHubService
    {
        // TODO: Make 'NurseHack4Health' dynamic
        const string GITHUBURL = "https://api.github.com/orgs/NurseHack4Health/";
        const string ORG = "NurseHack4Health";
        const string USERAGENT = "WhiteZeus";

        private Github githubObj = new Github();
        private int RepoId { get; set; }

        public Github CreateRepoAndTeam(string name, string description)
        {
            CreateTeam(name);
            CreateRepo(name, description);

            return githubObj;
        }

        private async void CreateTeam(string name)
        {
            JObject jsonObject = null;
            var client = CreateHttpClient();

            try
            {
                string payload = JsonConvert.SerializeObject(new GitHubTeamPayload { name = name });
                HttpResponseMessage res = client.PostAsync(GITHUBURL + "teams", new StringContent(payload, Encoding.UTF8, "application/json")).Result;
                client.Dispose();
                jsonObject = await DeserializeResponse(res);

                // Team ID
                githubObj.TeamId = Int32.Parse(jsonObject["id"].ToString());
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);
            }

        }

        private async void CreateRepo(string name, string desc)
        {
            JObject jsonObject = null;
            var client = CreateHttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                string payload = JsonConvert.SerializeObject(new GitHubRepoPayload { name = name, description = desc, team_id = githubObj.TeamId, license_template = "mit", auto_init = true });
                HttpResponseMessage res = client.PostAsync(GITHUBURL + "repos", new StringContent(payload, Encoding.UTF8, "application/json")).Result;
                client.Dispose();
                jsonObject = await DeserializeResponse(res);
                githubObj.RepoId = Int32.Parse(jsonObject["id"].ToString());
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);
            }
        }

        public async void AddUser(string gitHubUser, long gitHubUserId, int teamId, string teamName)
        {
            int[] teamIds = { teamId };
            var userIsInOrg = await FindUserInOrg(gitHubUser, ORG);
            var ghslug = teamName.Replace(" ", "-"); //to match GitHub Slugname

            var client = CreateHttpClient();
            
            try
            {
                if (userIsInOrg)
                {
                    string payload = JsonConvert.SerializeObject(new GitHubUserPayload { team_slug = ghslug, username = gitHubUser });
                    HttpResponseMessage res = client.PutAsync(GITHUBURL + "teams/" + ghslug + "/memberships/" + gitHubUser, new StringContent(payload, Encoding.UTF8, "application/json")).Result;
                }
                else
                {
                    string payload = JsonConvert.SerializeObject(new GitHubUserPayload { team_ids = teamIds, invitee_id = gitHubUserId });
                    HttpResponseMessage res = client.PostAsync(GITHUBURL + "invitations", new StringContent(payload, Encoding.UTF8, "application/json")).Result;
                }
            } catch 
            {
                throw new GitHubException();
            }
            
            client.Dispose();
        }

        //public async void AddUserToExistingTeam(string teamName, string gitHubUser)
        //{
        //    var client = CreateHttpClient();
        //    string payload = JsonConvert.SerializeObject(new GitHubExistingUserPayload { team_slug = teamName, username = gitHubUser });
        //    HttpResponseMessage res = client.PutAsync(GITHUBURL + "teams/" + teamName + "/memberships/" + gitHubUser, new StringContent(payload, Encoding.UTF8, "application/json")).Result;
        //    JObject jObject = await DeserializeResponse(res);
        //}

        //public async void AddUserToANewTeam (long gitHubUserId, int teamId)
        //{
        //    int[] teamIds = { teamId };

        //    var client = CreateHttpClient();
        //    client.DefaultRequestHeaders.Add("Authorization", TOKEN);
        //    client.DefaultRequestHeaders.Add("User-Agent", USERAGENT);
        //    string payload = JsonConvert.SerializeObject(new GitHubUserPayload { team_ids = teamIds, invitee_id = gitHubUserId });
        //    HttpResponseMessage res = client.PostAsync(GITHUBURL + "invitations", new StringContent(payload, Encoding.UTF8, "application/json")).Result;
        //}

        private async Task<bool> FindUserInOrg(string githubUser, string org)
        {
            JObject jsonObject;
            var client = CreateHttpClient();
            HttpResponseMessage res = client.GetAsync(GITHUBURL + "memberships/" + githubUser).Result;
            jsonObject = await DeserializeResponse(res);

            if(jsonObject["message"] != null)
            {
                return false;                
            } else
            {
                var state = jsonObject["state"].ToString();

                if (state == "active")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", UtilConst.GitHubToken);
            client.DefaultRequestHeaders.Add("User-Agent", USERAGENT);
            return client;
        }

        private async Task<JObject> DeserializeResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(json) as JObject;
        }

        [Serializable]
        private class GitHubException : Exception
        {
            public GitHubException()
            {
            }

            public GitHubException(string message) : base(message)
            {
                Console.WriteLine(message);
            }

            public GitHubException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected GitHubException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
