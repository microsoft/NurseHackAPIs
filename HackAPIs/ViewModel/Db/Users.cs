﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class Users
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
        public Boolean UserOptOut { get; set; } = false;
        public long GitHubId { get; set;}
        public string GitHubUser { get; set; }
    }
}
