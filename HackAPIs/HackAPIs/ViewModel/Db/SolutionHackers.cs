using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class HackerExpanded
    {
        public string name { get; set; }
        public int islead { get; set; }
    }
    public class SolutionHackers
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public virtual ArrayList UserID { get; set; }
        public virtual List<HackerExpanded> Hackers { get; set; }
    }
}
