using Microsoft.Extensions.Options;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Octokit;

namespace HackAPIs.Services.Util
{
    public class GitHubService
    {
        private readonly GitHubClient _gitClient;
        private GitHubServiceOptions _config;

        public GitHubService(IOptions<GitHubServiceOptions> options, GitHubClient client)
        {
            _config = options.Value;
            _gitClient = client;
        }

        public async Task<(int,long)> CreateRepoAndTeam(string name, string description)
        {
            try
            {
                int teamId = await CreateTeam(name);
                long repoId = await CreateRepo(name, description, teamId);

                return (teamId, repoId);
            }
            catch (Exception)
            {
                return (-1, -1);
            }
        }

        public async Task<int> CreateTeam(string name)
        {
            try
            {
                var team = await _gitClient.Organization.Team.Create(_config.Org, new NewTeam(name));
                return team.Id;
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);
            }

        }

        private async Task<long> CreateRepo(string name, string desc, int teamId)
        {
            try
            {
                var createRepo = new NewRepository(name);
                createRepo.TeamId = teamId;
                var repo = await _gitClient.Repository.Create(_config.Org, createRepo);

                return repo.Id;
            } catch (Exception ex)
            {
                throw new GitHubException(ex.Message);                
            }
        }

        public async Task AddUser(string gitHubUser, long gitHubUserId, int teamId, string teamName)
        {
            var userIsInOrg = await _gitClient.Organization.Member.CheckMember(_config.Org, gitHubUser);

            try
            {
                if (userIsInOrg)
                {
                    await _gitClient.Organization.Team.AddOrEditMembership(teamId, gitHubUser, new UpdateTeamMembership(TeamRole.Member));
                }
                else
                {
                    await _gitClient.Organization.Member.AddOrUpdateOrganizationMembership(_config.Org, gitHubUser, new OrganizationMembershipUpdate { Role = MembershipRole.Member });
                }
            } catch 
            {
                throw new GitHubException();
            }
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
