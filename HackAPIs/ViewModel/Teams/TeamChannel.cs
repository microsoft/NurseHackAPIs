using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Teams
{
    public class TeamChannel
    {
        public string TeamID { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; } 
        public string ChannelDescription { get; set; }
        public string ChannelMessage { get; set; }
        public string ChannelWebURL { get; set; }
    }
}
