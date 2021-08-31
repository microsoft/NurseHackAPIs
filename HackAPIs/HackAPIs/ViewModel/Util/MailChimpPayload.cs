namespace HackAPIs.ViewModel.Util
{
    public class MailChimpPayload
    {
        public string email_address { get; set; }
        public string status { get; set; }
        public Fields merge_fields { get; set; }

    }

    public class Fields
    {
        public string FNAME { get; set; }
        public string LNAME { get; set; }
    }
}
