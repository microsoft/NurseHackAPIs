using HackAPIs.Db.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Db.Model
{
    public partial class tblUsers
    {
        [Key]
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public string UserRegEmail { get; set; }
        public string UserMSTeamsEmail { get; set; }
        public string UserDisplayName { get; set; }
        public string MySkills { get; set; }
        public string UserTimeCommitment { get; set; }
        public Boolean Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ADUserId { get; set; }
        public Boolean UserOptOut { get; set; } = false;
        public Boolean MSFTOptIn { get; set; } = false;
        public Boolean JNJOptIn { get; set; } = false;
        public Boolean SONSIELOptIn { get; set; } = false;
        public string GitHubUser { get; set; }
        public long GitHubId { get; set; }
        public string MailchimpId { get; set; }

        public virtual ICollection<tblUserSkillMatch> tblUserSkillMatch { get; set; }
        public virtual ICollection<tblTeamHackers> tblTeamHackers { get; set; }

        public tblUsers()
        {
            tblUserSkillMatch = new HashSet<tblUserSkillMatch>();
            tblTeamHackers = new HashSet<tblTeamHackers>();
        }

    }
}
