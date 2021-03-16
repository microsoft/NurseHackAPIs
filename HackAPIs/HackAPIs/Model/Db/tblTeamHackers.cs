﻿using HackAPIs.Services.Db.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Services.Db.model
{
    public partial class tblTeamHackers
    {
        [Key]
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int IsLead { get; set; }
       

        public virtual tblUsers tblUsers { get; set; }
        public virtual tblTeams tblTeams { get; set; }
    }
}
