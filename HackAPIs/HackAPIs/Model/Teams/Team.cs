using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Teams
{
    public class Team
    {
        public string ID { get; set; }
        public string name { get; set; }
        public List<Member> Members { get; set; }
        public string Role { get; set; }
    }
}
