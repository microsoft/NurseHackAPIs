using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class UserTeams
    {
       
        public int UserId { get; set; }
        public virtual ArrayList TeamId { get; set; }
    }
}
