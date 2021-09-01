using HackAPIs.Db.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class TeamHackers
    {
        [Key]
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int IsLead { get; set; }


        public virtual TblUsers tblUsers { get; set; }
        public virtual TblTeams tblTeams { get; set; }
    }
}
