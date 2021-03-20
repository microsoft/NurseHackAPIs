using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HackAPIs.Services.Util
{
    public class EmailService
    {
        static bool mailSent = false;
        public string SendEmail(UserEmail userEmail)
        {
            string rtn = "";

            try
            {
                SmtpClient client = new SmtpClient(userEmail.SMTPAddress, userEmail.SMPTPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(userEmail.SMTPUser, userEmail.SMTPPassword);

                MailAddress sender = new MailAddress(userEmail.FromAddress, userEmail.FromDisplayName);

                MailAddress receiver = new MailAddress(userEmail.ToAddress);

                MailMessage message = new MailMessage(sender, receiver);
                message.IsBodyHtml = userEmail.IsHtmlBody;

                BlobStorageService blobStorageService = new BlobStorageService();
                BlobStorage blobStorage = new BlobStorage { Connection = UtilConst.StorageConn, Container = UtilConst.Container, Blob = UtilConst.Blob };
                string emailBody = blobStorageService.GetBlob(blobStorage);

                emailBody = emailBody.Replace("{DisplayName}", userEmail.UserName);
                emailBody = emailBody.Replace("{Title}", userEmail.Title);
                emailBody = emailBody.Replace("{Url}", userEmail.URL);
                emailBody = emailBody.Replace("{Description}", userEmail.Description);

                message.Body = emailBody;
                message.Subject = userEmail.Subject;

                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                string userState = userEmail.State;
                client.SendAsync(message, userState);
                rtn = "Email was sent successfully";
            } catch (Exception ex)
            {
                rtn = "Failed to sent the email";
            }

            return rtn;
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
    }
}
