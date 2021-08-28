using HackAPIs.ViewModel.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class GitHubService
    {
        private readonly HttpClient _httpClient;
        private GitHubServiceOptions _config;

        public GitHubService(HttpClient client, IOptions<GitHubServiceOptions> options)
        {
            _httpClient = client;
            _config = options.Value;
        }

        public async Task<Github> CreateRepoAndTeam(string name, string description)
        {
            var result = new Github();
            result.TeamId = await CreateTeam(name);
            result.RepoId = await CreateRepo(name, description, result.TeamId);

            return result;
        }

        private async Task<int> CreateTeam(string name)
        {
            try
            {
                string payload = JsonConvert.SerializeObject(new GitHubTeamPayload { name = name });
                HttpResponseMessage res = await _httpClient.PostAsync(_config.Url + "teams", new StringContent(payload, Encoding.UTF8, "application/json"));
                var jsonObject = await DeserializeResponse(res);

                // Team ID
                var teamId = Int32.Parse(jsonObject["id"].ToString());
                return teamId;
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);
            }

        }

        private async Task<int> CreateRepo(string name, string desc, int teamId)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            try
            {
                string payload = JsonConvert.SerializeObject(new GitHubRepoPayload { name = name, description = desc, team_id = teamId, license_template = "mit", auto_init = true });
                HttpResponseMessage res = await _httpClient.PostAsync(_config.Url + "repos", new StringContent(payload, Encoding.UTF8, "application/json"));
                var jsonObject = await DeserializeResponse(res);
                var id = Int32.Parse(jsonObject["id"].ToString());
                return id;
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);                
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Remove("Accept");
            }
        }

        public async Task AddUser(string gitHubUser, long gitHubUserId, int teamId, string teamName)
        {
            int[] teamIds = { teamId };
            var userIsInOrg = await FindUserInOrg(gitHubUser, _config.Org);
            var ghslug = teamName.Replace(" ", "-"); //to match GitHub Slugname

            try
            {
                if (userIsInOrg)
                {
                    string payload = JsonConvert.SerializeObject(new GitHubUserPayload { team_slug = ghslug, username = gitHubUser, role = "member"});
                    HttpResponseMessage res = await _httpClient.PutAsync(_config.Url + "teams/" + ghslug + "/memberships/" + gitHubUser, new StringContent(payload, Encoding.UTF8, "application/json"));
                }
                else
                {
                    string payload = JsonConvert.SerializeObject(new GitHubUserPayload { team_ids = teamIds, invitee_id = gitHubUserId });
                    HttpResponseMessage res = await _httpClient.PostAsync(_config.Url + "invitations", new StringContent(payload, Encoding.UTF8, "application/json"));
                }
            } catch 
            {
                throw new GitHubException();
            }
        }

        private async Task<bool> FindUserInOrg(string githubUser, string org)
        {
            HttpResponseMessage res = await _httpClient.GetAsync(_config.Url + "memberships/" + githubUser);
            var jsonObject = await DeserializeResponse(res);

            if(jsonObject["message"] != null)
            {
                return false;                
            } else
            {
                var state = jsonObject["state"].ToString();
                return state == "active" ? true : false;
            }
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
