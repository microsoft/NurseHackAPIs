using HackAPIs.Model.Db;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Util;
using HackAPIs.ViewModel;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Email;
using HackAPIs.ViewModel.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/Log")]
    [ApiController]
    [Authorize]
    public class LogController : Controller
    {
        private readonly IDataRepositoy<TblLog, Log> _dataRepositoryLog;

        public LogController(IDataRepositoy<TblLog, Log> dataRepositoyLog)
        {
            _dataRepositoryLog = dataRepositoyLog;
        }

        [HttpPost("save", Name = "save")]
        public async Task<string> log()
        {
            TblLog log = new TblLog
            {
                Label = "100",
                Description = "User",
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            _dataRepositoryLog.Add(log);
            return await Task.FromResult("success");
        }
    }
    
}
