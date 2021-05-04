using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Util
{
    public class GitHubTeamPayload
    {
        public string name { get; set; }
        public string privacy { get; set; } = "closed";
        public string permission { get; set; } = "admin";
    }
}
