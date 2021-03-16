using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HackAPIs.Services.Db;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackAPIs.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamsController : ControllerBase
    {

        private readonly ILogger<TeamsController> _logger;
        private readonly NurseHackContext _dbContext;

        public TeamsController(ILogger<TeamsController> logger, NurseHackContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }

        [HttpGet]
        public async Task<List<Skills>> Info()
        {
            List<Skills> skills = new List<Skills>();

            Skills s = new Skills { SkillId = 1, SkillName = "Welcome to NurseHack API Portal" };
            skills.Add(s);
            return skills;
        }

        /*
         * Returns a list of all users (members and guests) in the Azure AD domain
         */

        [HttpGet("domainusers")]
        public async Task<string> GetUsers()
        {
            TeamsService teamService = new TeamsService();

            string json = JsonConvert.SerializeObject(await teamService.GetDomainUsers());
            return json;

        }

        /*
        * Create a Azure AD domain member
        */

        [HttpPost("admember")]
        public async Task<string> CreateADMember(MemberUser member)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.CreateMemberUser(member));
            return json;

        }

        /*
        * Delete a Azure AD domain member
        */

        [HttpDelete("{id}")]
        public async Task<string> DeleteADMember(string Id)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.DeleteUser(Id));
            return json;

        }

        /*
       * Add and send an invite to a Guest User in Azure AD domain
       */

        [HttpPost("guestmember")]
        public async Task<string> CreateGuestMember(GuestUser guest)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.InviteGuestUser(guest));
            return json;

        }
        /*
         *  Get all users of a Team
         */

        [HttpGet("teammembers/{teamID}")]
        public async Task<string> GetTeamUsers(string teamID)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.GetTeamMembers(teamID));
            return json;

        }

        /*
        *  Add a user to a Team
        *
        */

        [HttpPost("teammember")]
        public async Task<string> AddMember(TeamMember member)
        {
            string json = "";
            TeamsService teamService = new TeamsService();
            if (member.Role.Contains("owner"))
                json = JsonConvert.SerializeObject(await teamService.AddTeamOwner(member));
            else
                json = JsonConvert.SerializeObject(await teamService.AddTeamMember(member));
            return json;

        }

        /*
        *  Remove a user from a Team
        *
        */
        [HttpDelete("teammember")]
        public async Task<string> RemoveMember(TeamMember member)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.RemoveTeamMember(member));
            return json;

            // Unscribe the member from the MailChimp Audience list

        }

        /*
       *  Get a list of Channels from a Team
       *
       */
        [HttpGet("channels/{teamID}")]
        public async Task<string> GetTeamChannels(string teamID)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.GetTeamChannels(teamID));
            return json;

        }


        /*
           *  Get a Team Channel Information
           *
           */
        [HttpGet("channel")]
        public async Task<string> GetTeamChannelInfo(TeamChannel teamChannel)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.GetChannelInfo(teamChannel));
            return json;
        }



        /*
        *  Create a Team Channel
        *
        */

        [HttpPost("channel")]
        public async Task<string> CreateChannel(TeamChannel teamChannel)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.CreateTeamChannel(teamChannel));
            return json;

        }


        /*
        *  Remove a Team Channel
        *
        */
        [HttpDelete("channel")]
        public async Task<string> DeleteChannel(TeamChannel teamChannel)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.DeleteTeamChannel(teamChannel));
            return json;

        }

        /*
        *  Create a Team Channel
        *
        */

        [HttpPost("channel/message")]
        public async Task<string> SendChannelMessage(TeamChannel teamChannel)
        {
            TeamsService teamService = new TeamsService();
            string json = JsonConvert.SerializeObject(await teamService.SendMessageToChannel(teamChannel));
            return json;

        }

    }
}
