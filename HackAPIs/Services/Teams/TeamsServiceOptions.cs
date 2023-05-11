using System.Collections.Generic;

namespace HackAPIs.Services.Teams
{
    public class TeamsServiceOptions
    {
        public string TeamDomain { get; set; }
        public List<string> TeamIds { get; set; }
        public bool ChannelAutoCreate { get; set; }
    }
}