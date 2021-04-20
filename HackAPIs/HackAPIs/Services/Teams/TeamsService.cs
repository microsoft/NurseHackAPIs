using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackAPIs.Services.Teams
{
    public class TeamsService : BaseTeamService
    {
     

        private readonly IDataRepositoy<tblUsers, Users> _dataRepository;

        public TeamsService()
        {

        }
        public TeamsService(IDataRepositoy<tblUsers, Users> dataRepositoy)
        {
            _dataRepository = dataRepositoy;
        }

        /*
            Get all users of the domain including members and guests
        */
        public async Task<JObject> GetDomainUsers()
        {
            string urlExt = "v1.0/users";
            JObject json = await RunAsync(urlExt, HttpMethodType.Get, null);
            return json;
        }

        /*
            Create a Azure AD domain member
        */
        public async Task<JObject> CreateMemberUser(MemberUser user)
        {
            string urlExt = "v1.0/users";

            Hashtable postPayload = new Hashtable();
            postPayload.Add("accountEnabled", "true");
            postPayload.Add("displayName", user.DisplayName);
            postPayload.Add("mailNickname", TeamDomain);
            postPayload.Add("userPrincipalName", user.PrincipalName+"@"+TeamDomain);
            Hashtable passPayLoad = new Hashtable();
            passPayLoad.Add("forceChangePasswordNextSignIn", "true");
            passPayLoad.Add("password", user.Password);
            postPayload.Add("passwordProfile", passPayLoad);
            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");


            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            return json;
        }

        /*
            Invite a guest user to the Azure AD domain
        */
        public async Task<GuestUser> InviteGuestUser(GuestUser user)
        {
            
            string urlExt = "v1.0/invitations";

            Hashtable postPayload = new Hashtable();
            postPayload.Add("inviteRedirectUrl", user.InviteRedirectUrl);
            postPayload.Add("invitedUserEmailAddress", user.InvitedUserEmailAddress);

            var output = JsonConvert.SerializeObject(postPayload); 
            var data = new StringContent(output, Encoding.UTF8, "application/json");
            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            string invitedUserId = (string)json["invitedUser"]["id"];
            user.ADUserId = invitedUserId;
            try
            {
                
                TeamMember member = new TeamMember();
                member.UserID = invitedUserId;
                member.TeamID = NurseHackTeam1;
                json = await AddTeamMember(member);
                member.TeamID = NurseHackTeam2;
                json = await AddTeamMember(member);

            } catch (Exception ex)
            {
                
            }
           
         return user;
        }


        /*
           Updates Azure AD Member
        */
        public async Task<JObject> UpdateMembers(string userId, string displayName)
        {
            string urlExt = "v1.0/users/" + userId;

            Hashtable postPayload = new Hashtable();
            
            postPayload.Add("displayName", displayName);
            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");

            JObject json = await RunAsync(urlExt, HttpMethodType.Patch, data);
            return json;
        }

        /*
            Get all users (includes Azure AD members and guests) of a Team including Team Owners and Members
        */
        public async Task<JObject> GetTeamMembers(string teamID)
        {
            string urlExt = "beta/groups/" + teamID + "/members";
            JObject json = await RunAsync(urlExt, HttpMethodType.Get, null);
            return json;
        }

        /*
            Add a user (Azure AD member or guest) to a Team as Member of a Team
        */
        public async Task<JObject> AddTeamMember(TeamMember teamMember)
        {
            string urlExt = "beta/groups/" + teamMember.TeamID + "/members/$ref";

            Hashtable postPayload = new Hashtable();
            string userURL = "https://graph.microsoft.com/beta/directoryObjects/" + teamMember.UserID;
            postPayload.Add("@odata.id", userURL);
            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");

            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            return json;
        }

        /*
           Add a user (Azure AD member) to a Team as Owner of a Team
       */
        public async Task<JObject> AddTeamOwner(TeamMember teamMember)
        {
            string urlExt = "beta/groups/" + teamMember.TeamID + "/owners/$ref";

            Hashtable postPayload = new Hashtable();
            string userURL = "https://graph.microsoft.com/beta/users/" + teamMember.UserID;
            postPayload.Add("@odata.id", userURL);
            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");

            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            return json;
        }

        /*
            Delete a user from the Azure AD
        */

        public async Task<JObject> DeleteUser(string UserID)
        {
            string urlExt = "v1.0/users/" + UserID;
            JObject json = await RunAsync(urlExt, HttpMethodType.Delete, null);
            return json;
        }

        /*
            Remove a Member (Azure AD member or guest) from a Team
        */

        public async Task<JObject> RemoveTeamMember(TeamMember teamMember)
        {
            teamMember.TeamID = NurseHackTeam1;
            string urlExt = "v1.0/groups/" + teamMember.TeamID + "/members/"+teamMember.MemberID+"/$ref";
            JObject json = await RunAsync(urlExt, HttpMethodType.Delete, null);
            /*
            teamMember.TeamID = NurseHackTeam2;
            urlExt = "v1.0/groups/" + teamMember.TeamID + "/members/" + teamMember.MemberID + "/$ref";
            json = await RunAsync(urlExt, HttpMethodType.Delete, null);
            */
            return json;
        }

        /*
            Remove a Owner (Azure AD member) from a Team
        */

        public async Task<JObject> RemoveTeamOwner(TeamMember teamMember)
        {
            string urlExt = "/beta/groups/" + teamMember.TeamID + "/owners/" + teamMember.MemberID + "/$ref";
            JObject json = await RunAsync(urlExt, HttpMethodType.Delete, null);
            return json;
        }

        /*
         Get list of Team Channels
        */
        public async Task<JObject> GetTeamChannels(string teamID)
        {
            string urlExt = "beta/teams/" + teamID + "/channels";
            JObject json = await RunAsync(urlExt, HttpMethodType.Get, null);
         //   json.GetValue("dd");
            return json;
        }

        /*
         Get a Team Channel information
        */
        public async Task<JObject> GetChannelInfo(TeamChannel teamChannel)
        {
            string urlExt = "beta/teams/" + teamChannel.TeamID + "/channels/"+teamChannel.ChannelId;
            JObject json = await RunAsync(urlExt, HttpMethodType.Get, null);
            return json;
        }


        /*
           Create a Teams Channel
       */
        public async Task<TeamChannel> CreateTeamChannel(TeamChannel teamChannel)
        {
            teamChannel.TeamID = NurseHackTeam1;
            string urlExt = "beta/teams/" + teamChannel.TeamID + "/channels";

            Hashtable postPayload = new Hashtable();
            
            postPayload.Add("displayName", teamChannel.ChannelName);
            postPayload.Add("description", teamChannel.ChannelDescription);
            postPayload.Add("membershipType", "standard");

            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");

            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            string channelWebURL = (string)json["webUrl"];
            teamChannel.ChannelWebURL = channelWebURL;

            return teamChannel;
        }

        /*
            Delete a Teams Channel
        */

        public async Task<JObject> DeleteTeamChannel(TeamChannel teamChannel)
        {
            string urlExt = "/beta/teams/" + teamChannel.TeamID + "/channels/" + teamChannel.ChannelId;
            JObject json = await RunAsync(urlExt, HttpMethodType.Delete, null);
            return json;
        }

        /*
          Send a message to a Teams Channel
         */
        public async Task<JObject> SendMessageToChannel(TeamChannel teamChannel)
        {
            string urlExt = "beta/teams/" + teamChannel.TeamID + "/channels/"+teamChannel.ChannelId+"/messages";

            Hashtable postPayload = new Hashtable();
            Hashtable contenPayLoad = new Hashtable();
            contenPayLoad.Add("content", teamChannel.ChannelMessage);
            postPayload.Add("body", contenPayLoad);

            var output = JsonConvert.SerializeObject(postPayload);
            var data = new StringContent(output, Encoding.UTF8, "application/json");

            JObject json = await RunAsync(urlExt, HttpMethodType.Post, data);
            return json;
        }
    }
}
