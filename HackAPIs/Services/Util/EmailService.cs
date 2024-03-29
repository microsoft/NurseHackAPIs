﻿using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Email;
using HackAPIs.ViewModel.Util;
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
        public string SendEmail(UserEmail userEmail)
        {
            string rtn = "";

            try
            {
                SmtpClient client = new SmtpClient(userEmail.SMTPAddress, userEmail.SMPTPort);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
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
            } catch (Exception)
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
        }

        public void InvokeEmail(string email, string userName)
        {
            try
            {
                UserEmail userEmail = new UserEmail
                {
                    UserName = userName,
                    Title = "NurseHack4Health",
                    URL = "https://nursehack4health.org",
                    Description = "Welcome to NurseHack4Health!",
                    Subject = "Welcome to NurseHack4Health!",
                    State = "Test Message",
                    FromAddress = UtilConst.SMTPFromAddress,
                    ToAddress = email,
                    SMTPAddress = UtilConst.SMTP,
                    SMTPUser = UtilConst.SMTPUser,
                    SMTPPassword = UtilConst.SMTPPassword,
                    IsHtmlBody = true,
                    FromDisplayName = "NurseHack4Health"
                };

                SendEmail(userEmail);
            }
            catch (Exception) { /* YUMMY exception swallowing in my tummy!! */ }
        }
    }
}
