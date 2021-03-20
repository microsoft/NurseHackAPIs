using HackAPIs.Services.Util;
using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Email;
using Microsoft.AspNetCore.Mvc;
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
    }
}
