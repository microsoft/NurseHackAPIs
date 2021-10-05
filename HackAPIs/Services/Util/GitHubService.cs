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
                var team = await _gitClient.Organization.Team.Create(_config.Org, new NewTeam(name) 
                    { 
                        Privacy = TeamPrivacy.Closed, 
                        Permission = Permission.Admin 
                    });
                return team.Id;
            } 
            catch (Exception ex)
            {
                throw new GitHubException("Error Creating GitHub Team.", ex);
            }

        }

        private async Task<long> CreateRepo(string name, string desc, int teamId)
        {
            try
            {
                var repo = await _gitClient.Repository.Create(_config.Org, new NewRepository(name) { TeamId = teamId, AutoInit = true, LicenseTemplate = "mit" });

                return repo.Id;
            } 
            catch (Exception ex)
            {
                throw new GitHubException("Error creating GitHub repository.", ex);                
            }
        }

        public async Task AddUser(string gitHubUser, long gitHubUserId, int teamId)
        {
            try
            { 
                var userIsInOrg = await _gitClient.Organization.Member.CheckMember(_config.Org, gitHubUser);

                // Check if user is member of GH Org.
                if (!userIsInOrg)
                {
                    // User not found. Add to org.
                    await _gitClient.Organization.Member.AddOrUpdateOrganizationMembership(_config.Org, gitHubUser, new OrganizationMembershipUpdate { Role = MembershipRole.Member });
                }

                // Add user to team
                await _gitClient.Organization.Team.AddOrEditMembership(teamId, gitHubUser, new UpdateTeamMembership(TeamRole.Member));
            } 
            catch (Exception ex)
            {
                throw new GitHubException("Error adding user to GitHub Team.", ex);
            }
        }
        
    }

    [Serializable]
    public class GitHubException : Exception
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
