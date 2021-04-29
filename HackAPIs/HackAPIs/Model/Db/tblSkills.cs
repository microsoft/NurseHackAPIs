using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Db.Model
{
    public class tblSkills
    {
        [Key]
        public int SkillId { get; set; }

       
        public string SkillName { get; set; }
   //     public IList<tblTeamSkillMatch> tblTeamSkillMatch { get; set; }
    }
}
