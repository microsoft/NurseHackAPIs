﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class SolutionSkills
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public virtual ArrayList SkillId { get; set; }
        
    }
}
