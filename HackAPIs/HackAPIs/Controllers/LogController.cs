using HackAPIs.Services.Util;
using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Email;
using HackAPIs.ViewModel.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Controllers
{
    [Route("api/Log")]
    [ApiController]
    public class LogController : Controller
    {
        [HttpGet("blob", Name = "GetBlobFile")]
        public string Index()
        {
            /*
             BlobStorageService blobStorageService = new BlobStorageService();
             BlobStorage blobStorage = new BlobStorage { Connection = UtilConst.StorageConn, Container = UtilConst.Container, Blob = UtilConst.Blob };
             return blobStorageService.GetBlob(blobStorage);
            */
            EmailService emailService = new EmailService();

            UserEmail userEmail = new UserEmail
            {
                UserName = "Guru",
                Title = "NurseHack4Health",
                URL = "https://nursehack4health.org",
                Description = "New Registration",
                Subject = "Hello From HackAPI",
                State = "Test Message",
                FromAddress = UtilConst.SMTPFromAddress,
                ToAddress = "gurub100@gmail.com",
                SMTPAddress = UtilConst.SMTP,
                SMTPUser = UtilConst.SMTPUser,
                SMTPPassword = UtilConst.SMTPPassword,
                IsHtmlBody = true,
                FromDisplayName = "Email"
            };

            return emailService.SendEmail(userEmail);
        }

        [HttpGet("subscribe", Name = "MailChimp")]
        public async Task<string> MailChimp()
        {
            /*
             BlobStorageService blobStorageService = new BlobStorageService();
             BlobStorage blobStorage = new BlobStorage { Connection = UtilConst.StorageConn, Container = UtilConst.Container, Blob = UtilConst.Blob };
             return blobStorageService.GetBlob(blobStorage);
            */
            MailChimpService mailChimpService = new MailChimpService();

            MailChimp mailChimp = new MailChimp
            {
                Audience = UtilConst.MailChimpAudience,
                Key = UtilConst.MailChimpKey,
                URL = UtilConst.MailChimpURL,
                User = UtilConst.MailChimpUser,
                mailChimpPayload = new MailChimpPayload
                {
                    email_address = "dupton2000@hotmail.com",
                    status = "subscribed",
       //             status_if_new = "subscribed",
                    merge_fields = new Fields
                    {
                        FNAME = "Raja",
                        LNAME = "Rao"
                    }
                }

            };

            //            JObject jObject =  await mailChimpService.UpdateMemberInList(mailChimp);
            //            string id = jObject["id"].ToString();

            string id = await mailChimpService.InvokeMailChimp("dupton2000@hotmail.com", "Dave",
                             "Dave Hotmail","", "subscribed", 1);
            //            return mailChimpService.GetMembers(mailChimp);
            return id;

            //       return mailChimpService.MemberSubcribe(mailChimp);
            //            return emailService.SendEmail(userEmail);
        }
    }
    
}
