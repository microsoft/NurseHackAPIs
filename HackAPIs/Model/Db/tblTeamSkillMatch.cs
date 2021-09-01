using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Db.Model
{
    public partial class TblTeamSkillMatch
    {
        [Key]
        public int TeamId { get; set; }
        public virtual TblTeams tblTeams { get; set; }
        [Key]
        public int SkillId { get; set; }
        public virtual TblSkills tblSkills { get; set; }
    }
}
