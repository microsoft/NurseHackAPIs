using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HackAPIs.Model.Db
{
    public class tblLog
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string Label { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; } 
    }
}
