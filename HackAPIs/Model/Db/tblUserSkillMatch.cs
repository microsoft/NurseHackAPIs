using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Db.Model
{
    public partial class TblUserSkillMatch
    {
        [Key]
        public int UserId { get; set; }
        public virtual TblUsers tblUsers { get; set; }
        [Key]
        public int SkillId { get; set; }
        public virtual TblSkills tblSkills { get; set; }

        
    }
}
