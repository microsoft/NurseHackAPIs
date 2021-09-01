using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
   
    public class RegLinks
    {
        [Key]
        public int RegLinkId { get; set; }
        public Guid UniqueCode { get; set; }
        public string IntendedEmail { get; set; }
        public string UsedByEmail { get; set; }
        public string IsUsed { get; set; }
        public string UserRole { get; set; }

  
}
    
}
