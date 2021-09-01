using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Teams
{
    public class Channel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Team team {get; set;}
    }
}
