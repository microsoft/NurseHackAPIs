using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Db.Model
{
    public partial class tblUserSkillMatch
    {
        [Key]
        public int UserId { get; set; }
        public virtual tblUsers tblUsers { get; set; }
        [Key]
        public int SkillId { get; set; }
        public virtual tblSkills tblSkills { get; set; }

        
    }
}
