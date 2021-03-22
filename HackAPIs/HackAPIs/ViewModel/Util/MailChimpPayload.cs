using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Util
{
    public class MailChimpPayload
    {
        public string email_address { get; set; }
 //       public string status_if_new { get; set; }
        public string status { get; set; }
        public Fields merge_fields { get; set; }

    }

    public class Fields
    {
        public string FNAME { get; set; }
        public string LNAME { get; set; }
    }
}
