# Create Azure AD App Registration for HackAPI

1. Browse to Azure Portal and wwitch to tenant where App Registration should be created.
1. Choose App Registration blade.
1. Create a new App registration
1. Select `API Permissions` blade and then `Add a Permission`
1. Choose Microsoft Graph, then Delegated Permissions, and add the following
    * Channel.Create
    * Channel.Delete.All
    * Channel.ReadBasic.All
    * ChannelMember.ReadWrite.All
    * ChannelMessage.Read.All
    * ChannelSettings.Read.All
    * ChannelSettings.ReadWrite.All
    * Directory.ReadWrite.All
    * Group.ReadWrite.All
    * GroupMember.ReadWrite.All
    * PrivilegedAccess.ReadWrite.AzureADGroup
    * Team.ReadBasic.All
    * TeamMember.ReadWrite.All
    * TeamMember.ReadWriteNonOwnerRole.All
    * TeamsActivity.Read.All
    * TeamsActivity.Send
    * TeamsApp.Read.All
    * TeamSettings.ReadWrite.All
    * User.Invite.All
    * User.ReadWrite.All