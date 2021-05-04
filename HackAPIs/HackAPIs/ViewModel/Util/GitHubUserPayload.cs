using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Util
{
    public class GitHubUserPayload
    {
        public string team_slug { get; set; }
        public string username { get; set; }
        public string role { get; set; } = "direct_member";
        public long invitee_id { get; set; }
        public Array team_ids { get; set; } = new Array[1];
    }
}
