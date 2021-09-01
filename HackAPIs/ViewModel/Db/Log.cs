using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Db
{
    public class Log
    {
       
        public int ID { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string Label { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
