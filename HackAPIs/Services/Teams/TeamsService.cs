using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Extensions.Options;

namespace HackAPIs.Services.Teams
{
    public class TeamsService 
    {
        private readonly GraphServiceClient _graphClient;
        private readonly TeamsServiceOptions _config;

        public TeamsService(TeamsServiceOptions options, GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
            _config = options;
        }

        /*
            Get all users of the domain including members and guests WUUUUUUUUTTTTT!!?!!!?!!?!!????
        */
        public async Task<IEnumerable<User>> GetDomainUsers()
        {
            var results = await _graphClient.Users.Request().GetAsync();
            return results.ToList();
        }

        /*
            Create a Azure AD domain member
        */
        public async Task<User> CreateMemberUser(MemberUser user)
        {
            var graphUser = new User
            {
                AccountEnabled = true,
                DisplayName = user.DisplayName,
                MailNickname = _config.TeamDomain,
                UserPrincipalName = string.Concat(user.PrincipalName, "@", _config.TeamDomain),
                PasswordProfile = new PasswordProfile { Password = user.Password, ForceChangePasswordNextSignIn = true }
            };

            return await _graphClient.Users.Request().AddAsync(graphUser);
        }

        /*
            Invite a guest user to the Azure AD domain
        */
        public async Task<Invitation> InviteGuestUser(GuestUser user)
        {
            var invite = new Invitation
            {
                InviteRedirectUrl = user.InviteRedirectUrl,
                InvitedUserEmailAddress = user.InvitedUserEmailAddress,
                SendInvitationMessage = true,
                InvitedUserDisplayName = user.DisplayName
            };
            var result = await _graphClient.Invitations.Request().AddAsync(invite);
            await AddTeamMember(_config.MSTeam1, result.InvitedUser.Id);
            await AddTeamMember(_config.MSTeam2, result.InvitedUser.Id);

            return result;
        }

        /*
            Get all users (includes Azure AD members and guests) of a Team including Team Owners and Members
        */
        public async Task<IEnumerable<DirectoryObject>> GetTeamMembers(string teamId)
        {
            var results = await _graphClient.Groups[teamId].Members.Request().GetAsync();
            return results.ToList();
        }

        /*
            Add a user (Azure AD member or guest) to a Team as Member of a Team
        */
        public async Task AddTeamMember(string teamId, string userId)
        {
            await _graphClient.Groups[teamId].Members.References.Request().AddAsync(new DirectoryObject { Id = userId });
        }

        /*
           Add a user (Azure AD member) to a Team as Owner of a Team
       */
        public async Task AddTeamOwner(string teamId, string userId)
        {
            await _graphClient.Groups[teamId].Owners.References.Request().AddAsync(new DirectoryObject { Id = userId });
        }

        /*
            Delete a user from the Azure AD
        */

        public async Task DeleteUser(string userId)
        {
            await _graphClient.Users[userId].Request().DeleteAsync();
        }

        /*
            Remove a Member (Azure AD member or guest) from a Team
        */

        public async Task RemoveTeamMember(string teamId, string userId)
        {
            await _graphClient.Groups[teamId].Members[userId].Request().DeleteAsync();
        }

        /*
            Remove a Owner (Azure AD member) from a Team
        */

        public async Task RemoveTeamOwner(string teamId, string userId)
        {
            await _graphClient.Groups[teamId].Owners[userId].Request().DeleteAsync();
        }

        /*
         Get list of Team Channels
        */
        public async Task<IEnumerable<Channel>> GetTeamChannels(string teamId)
        {
            var result = await _graphClient.Teams[teamId].Channels.Request().GetAsync();
            return result.ToList();
        }

        /*
         Get a Team Channel information
        */
        public async Task<Channel> GetChannelInfo(string teamId, string channelId)
        {
            return await _graphClient.Teams[teamId].Channels[channelId].Request().GetAsync();
        }

        /*
           Create a Teams Channel
       */
        public async Task<Channel> CreateTeamChannel(string teamId, string displayName, string description, ChannelMembershipType memberType = ChannelMembershipType.Standard)
        {
            var channel = new Channel
            {
                DisplayName = displayName,
                Description = description,
                MembershipType = memberType
            };
            var result = await _graphClient.Teams[teamId].Channels.Request().AddAsync(channel);

            return result;
        }

        /*
            Delete a Teams Channel
        */

        public async Task DeleteTeamChannel(string teamId, string channelId)
        {
            await _graphClient.Teams[teamId].Channels[channelId].Request().DeleteAsync();
        }

        /*
          Send a message to a Teams Channel
         */
        public async Task<ChatMessage> SendMessageToChannel(string teamId, string channelId, string message)
        {
            var chat = new ChatMessage
            {
                Body = new ItemBody { Content = message, ContentType = BodyType.Text }
            };
            return await _graphClient.Teams[teamId].Channels[channelId].Messages.Request().AddAsync(chat);
        }
    }
}
