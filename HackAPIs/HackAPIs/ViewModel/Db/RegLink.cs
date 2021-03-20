using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class RegLinkShort
    { 
        public string UserRole { get; set; }
    }

    public class RegLink
    {
        [Key]
        public int RegLinkId { get; set; }
        public Guid UniqueCode { get; set; }
        public string IntendedEmail { get; set; }
        public string UsedByEmail { get; set; }
        public string IsUsed { get; set; }
        public string UserRole { get; set; }

    public HackAPIs.ViewModel.Db.RegLinkShort getShort()
    {
        return new HackAPIs.ViewModel.Db.RegLinkShort { UserRole=this.UserRole };
    }
}
    
}
