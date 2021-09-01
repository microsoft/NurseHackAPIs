using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HackAPIs.Services.Db;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/teams")]
    [ApiController]
    public class TeamsController : ControllerBase
    {

        private readonly ILogger<TeamsController> _logger;
        private readonly TeamsService _teamsService;

        public TeamsController(ILogger<TeamsController> logger, NurseHackContext dbContext, TeamsService teamsService)
        {
            _logger = logger;
            _teamsService = teamsService;
        }

        [HttpGet]
        public async Task<List<Skills>> Info()
        {
            List<Skills> skills = new List<Skills>();

            Skills s = new Skills { SkillId = 1, SkillName = "Welcome to NurseHack API Portal" };
            skills.Add(s);
            return await Task.FromResult(skills);
        }

        /*
         * Returns a list of all users (members and guests) in the Azure AD domain
         */

        [HttpGet("domainusers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var results = await _teamsService.GetDomainUsers();                
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        * Create a Azure AD domain member
        */

        [HttpPost("admember")]
        public async Task<IActionResult> CreateADMember(MemberUser member)
        {
            try
            {
                var user = await _teamsService.CreateMemberUser(member);
                return Created($"https://admember/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        * Delete a Azure AD domain member
        */

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteADMember(string userId)
        {
            try
            {
                await _teamsService.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
       * Add and send an invite to a Guest User in Azure AD domain
       */

        [HttpPost("guestmember")]
        public async Task<IActionResult> CreateGuestMember(GuestUser guest)
        {
            try
            {
                var result = await _teamsService.InviteGuestUser(guest);
                return Created($"https://guestmember/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
         *  Get all users of a Team
         */

        [HttpGet("teammembers/{teamID}")]
        public async Task<IActionResult> GetTeamUsers(string teamId)
        {
            try
            {
                var results = await _teamsService.GetTeamMembers(teamId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        *  Add a user to a Team
        *
        */

        [HttpPost("teammember")]
        public async Task<IActionResult> AddMember(TeamMember member)
        {
            if (string.IsNullOrEmpty(member.TeamID)) throw new ArgumentNullException("member.TeamID");
            if (string.IsNullOrEmpty(member.UserID)) throw new ArgumentNullException("member.UserID");
            if (string.IsNullOrEmpty(member.Role)) throw new ArgumentNullException("member.Role");

            try
            {
                if (member.Role.Contains("owner"))
                    await _teamsService.AddTeamOwner(member.TeamID, member.UserID);
                else
                    await _teamsService.AddTeamMember(member.TeamID, member.UserID);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        *  Remove a user from a Team
        *
        */
        [HttpDelete("teammember")]
        public async Task<IActionResult> RemoveMember(TeamMember member)
        {
            try
            {
                await _teamsService.RemoveTeamMember(member.TeamID, member.UserID);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Unscribe the member from the MailChimp Audience list

        }

        /*
       *  Get a list of Channels from a Team
       *
       */
        [HttpGet("channels/{teamID}")]
        public async Task<IActionResult> GetTeamChannels(string teamID)
        {
            try
            {
                var results = await _teamsService.GetTeamChannels(teamID);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*
           *  Get a Team Channel Information
           *
           */
        [HttpGet("channel")]
        public async Task<IActionResult> GetTeamChannelInfo(TeamChannel teamChannel)
        {
            ValidateTeamChannel(teamChannel);

            try
            {
                var result = await _teamsService.GetChannelInfo(teamChannel.TeamID, teamChannel.ChannelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /*
        *  Create a Team Channel
        *
        */

        [HttpPost("channel")]
        public async Task<IActionResult> CreateChannel(TeamChannel teamChannel)
        {
            ValidateTeamChannelCreate(teamChannel);

            try
            {
                var result = await _teamsService.CreateTeamChannel(
                    teamChannel.TeamID,
                    teamChannel.ChannelName,
                    teamChannel.ChannelDescription);
                return Created($"https://channel/{result.Id}", result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*
        *  Remove a Team Channel
        *
        */
        [HttpDelete("channel")]
        public async Task<IActionResult> DeleteChannel(TeamChannel teamChannel)
        {
            ValidateTeamChannel(teamChannel);

            try
            {
                await _teamsService.DeleteTeamChannel(teamChannel.TeamID, teamChannel.ChannelId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        *  Create a Team Channel Message
        *
        */

        [HttpPost("channel/message")]
        public async Task<IActionResult> SendChannelMessage(TeamChannel teamChannel)
        {
            ValidateTeamChannelMessage(teamChannel);

            try
            {
                var result = await _teamsService.SendMessageToChannel(teamChannel.TeamID, teamChannel.ChannelId, teamChannel.ChannelMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void ValidateTeamChannelMessage(TeamChannel teamChannel)
        {
            if (string.IsNullOrEmpty(teamChannel.TeamID)) throw new ArgumentNullException("teamChannel.TeamID");
            if (string.IsNullOrEmpty(teamChannel.ChannelName)) throw new ArgumentNullException("teamChannel.ChannelName");
            if (string.IsNullOrEmpty(teamChannel.ChannelMessage)) throw new ArgumentNullException("teamChannel.ChannelMessage");
        }

        private void ValidateTeamChannel(TeamChannel teamChannel)
        {
            if (string.IsNullOrEmpty(teamChannel.TeamID)) throw new ArgumentNullException("teamChannel.TeamID");
            if (string.IsNullOrEmpty(teamChannel.ChannelId)) throw new ArgumentNullException("teamChannel.ChannelId");
        }

        private void ValidateTeamChannelCreate(TeamChannel teamChannel)
        {
            if (string.IsNullOrEmpty(teamChannel.TeamID)) throw new ArgumentNullException("teamChannel.TeamID");
            if (string.IsNullOrEmpty(teamChannel.ChannelName)) throw new ArgumentNullException("teamChannel.ChannelName");
            if (string.IsNullOrEmpty(teamChannel.ChannelDescription)) throw new ArgumentNullException("teamChannel.ChannelDescription");
        }

    }
}
