using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class UserSkillMatch
    {
        public int UserId { get; set; }
        
        public virtual ArrayList SkillId { get; set; }
    }
}
