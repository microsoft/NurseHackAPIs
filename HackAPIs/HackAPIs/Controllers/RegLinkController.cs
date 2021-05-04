using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Db.Model;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using HackAPIs.Model.Db.DataManager;

namespace HackAPIs.Controllers
{
    [Route("api/reglink")]
    [ApiController]
    public class RegLinkController : Controller
    {
        private readonly IDataRepositoy<tblRegLink, RegLinks> _dataRepository;

        public RegLinkController(IDataRepositoy<tblRegLink, RegLinks> dataRepositoy)
        {
            _dataRepository = dataRepositoy;
        }

        [HttpPost("code", Name = "ValidateCode")]
        public IActionResult ValidateCode([FromBody] RegLinks regLink)
        {
            string errorMsg = "";
            string email = regLink.UsedByEmail;
            Guid code;
            bool isValidGuid = Guid.TryParse(regLink.UniqueCode.ToString(), out code);

            if (isValidGuid)
            {
                tblRegLink tblRegLink = ((RegLinkDataManager)_dataRepository).GetByCode(code);
                if(tblRegLink == null)
                {
                    errorMsg = "Provided code does not exist in the system.";
                } else if (!tblRegLink.IsUsed)
                {   tblRegLink finalEntity = new tblRegLink() { UsedByEmail = email };

                    _dataRepository.Update(tblRegLink, finalEntity, 0);
                    return new OkObjectResult(tblRegLink.UserRole);
                } else
                {
                    errorMsg = "Code Already Used";
                }
            }
            
            return Ok(new ErrorObj { ReturnError = errorMsg });            
        }
       
    }
}