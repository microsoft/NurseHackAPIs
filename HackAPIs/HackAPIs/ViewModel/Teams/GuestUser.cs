using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Teams
{
    public class GuestUser
    {
        
        public string InvitedUserEmailAddress { get; set; }
        
        public string InviteRedirectUrl { get; set; } = "https://myapps.microsoft.com";
        public int UserId { get; set; }
        public Boolean Active { get; set; }
        public string ADUserId { get; set; }
        public string DisplayName { get; set; }
    }
}
