using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Util
{
    public class GitHubRepoPayload
    {
        public bool auto_init { get; set; } = true;
        public string name { get; set; }
        public string description { get; set; }
        public string license_template { get; set; } = "mit";
        public int team_id { get; set; }
        //public string visibility { get; set; } = "public";
    }
}
