using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Util
{
    public class GitHubExistingUserPayload
    {
        public string team_slug { get; set; }
        public string username { get; set; }
        public string role { get; set; } = "direct_member";
    }
}
