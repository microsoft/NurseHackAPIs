using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class Solutions
    {
        
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string GithubURL { get; set; }
        public string ChallengeName { get; set; }
        public string SkillsWanted { get; set; }
        public string MSTeamsChannel { get; set; }
        public string MSLabEnvironment { get; set; }
        public string MSLabTenantName { get; set; }
        public string MSLabAzureUsername { get; set; }
        public string MSLabSPNAppId { get; set; }
        public string MSLabSPNAppObjectId { get; set; }
        public string MSLabSPNObjectId { get; set; }
        public string MSLabSPNDisplayName { get; set; }
        public string MSLabSPNKey { get; set; }
        public Boolean Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<SolutionSkills> SolutionSkills { get; set; }
        public virtual ICollection<SolutionHackers> SolutionHackers { get; set; }

    }
}
