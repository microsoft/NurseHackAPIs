# Create Azure AD App Registration for HackAPI

1. Browse to Azure Portal and wwitch to tenant where App Registration should be created.
1. Choose App Registration blade.
1. Create a new App registration
1. Select `API Permissions` blade and then `Add a Permission`
1. Choose Microsoft Graph, then Delegated Permissions, and add the following (see Manifest definition below)
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
1. Select `Expose an API` blade and then `Add a scope`
1. Scope name: *user_impersonation*; Consent: *Admin and users*; Display: *Read User*
1. Select `Certificates & Secrets` blade and then `New client secret`
1. Description: *HackAPISecret* (Be sure to capture the secret after creation)




Permissions to add to Manifest:
```json
"requiredResourceAccess": [
{
   "resourceAppId": "00000003-0000-0000-c000-000000000000",
   "resourceAccess": [
      {
         "id": "e1fe6dd8-ba31-4d61-89e7-88639da4683d",
         "type": "Scope"
      },
      {
         "id": "4437522e-9a86-4a41-a7da-e380edd4a97d",
         "type": "Role"
      },
      {
         "id": "35930dcf-aceb-4bd1-b99a-8ffed403c974",
         "type": "Role"
      },
      {
         "id": "3b55498e-47ec-484f-8136-9013221c06a9",
         "type": "Role"
      },
      {
         "id": "0121dc95-1b9f-4aed-8bac-58c5ac466691",
         "type": "Role"
      },
      {
         "id": "660b7406-55f1-41ca-a0ed-0b035e182f3e",
         "type": "Role"
      },
      {
         "id": "59a6b24b-4225-4393-8165-ebaec5f55d7a",
         "type": "Role"
      },
      {
         "id": "2280dda6-0bfd-44ee-a2f4-cb867cfc4c1e",
         "type": "Role"
      },
      {
         "id": "243cded2-bd16-4fd6-a953-ff8177894c3d",
         "type": "Role"
      },
      {
         "id": "c97b873f-f59f-49aa-8a0e-52b32d762124",
         "type": "Role"
      },
      {
         "id": "6a118a39-1227-45d4-af0c-ea7b40d210bc",
         "type": "Role"
      },
      {
         "id": "f3a65bd4-b703-46df-8f7e-0174fea562aa",
         "type": "Role"
      },
      {
         "id": "dbaae8cf-10b5-4b86-a4a1-f871c94c6695",
         "type": "Role"
      },
      {
         "id": "98830695-27a2-44f7-8c18-0c3ebc9698f6",
         "type": "Role"
      },
      {
         "id": "afdb422a-4b2a-4e07-a708-8ceed48196bf",
         "type": "Role"
      },
      {
         "id": "2f6817f8-7b12-4f0f-bc18-eeaf60705a9e",
         "type": "Role"
      },
      {
         "id": "01e37dc9-c035-40bd-b438-b2879c4870a6",
         "type": "Role"
      },
      {
         "id": "a267235f-af13-44dc-8385-c1dc93023186",
         "type": "Role"
      },
      {
         "id": "70dec828-f620-4914-aa83-a29117306807",
         "type": "Role"
      },
      {
         "id": "19dbc75e-c2e2-444c-a770-ec69d8559fc7",
         "type": "Role"
      },
      {
         "id": "7ab1d382-f21e-4acd-a863-ba3e13f7da61",
         "type": "Role"
      },
      {
         "id": "62a82d76-70ea-41e2-9197-370581804d09",
         "type": "Role"
      },
      {
         "id": "5b567255-7703-4780-807c-7be8301ae99b",
         "type": "Role"
      },
      {
         "id": "4d02b0cc-d90b-441f-8d82-4fb55c34d6bb",
         "type": "Role"
      },
      {
         "id": "7b2449af-6ccd-4f4d-9f78-e550c193f0d1",
         "type": "Role"
      },
      {
         "id": "741f803b-c850-494e-b5df-cde7c675a1ca",
         "type": "Role"
      },
      {
         "id": "09850681-111b-4a89-9bed-3f2cae46d706",
         "type": "Role"
      },
      {
         "id": "bdd80a03-d9bc-451d-b7c4-ce7c63fe3c8f",
         "type": "Role"
      },
      {
         "id": "242607bd-1d2c-432c-82eb-bdb27baa23ab",
         "type": "Role"
      }
   ]
}
```
