using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.ViewModel.Email
{
    public class UserEmail
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string State { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public Boolean IsHtmlBody { get; set; }
        public string TemplatePath { get; set; }
        public string SMTPAddress { get; set; } = "smtp.office365.com";
        public int SMPTPort { get; set; } = 587;
        public string SMTPPassword { get; set; }
        public string SMTPUser { get; set; }
    }
}
