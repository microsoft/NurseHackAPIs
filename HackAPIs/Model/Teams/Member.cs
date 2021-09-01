using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Teams
{
    public class Member
    {
        public string UserID { get; set; }
        public string memberID { get; set; }
        public List<Team> Teams { get; set; }
        public string Role { get; set; }
    }
}
