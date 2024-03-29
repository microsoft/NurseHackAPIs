﻿using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Db.Model;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using HackAPIs.Model.Db.DataManager;
using Microsoft.AspNetCore.Authorization;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/reglink")]
    [ApiController]
    [Authorize]
    public class RegLinkController : Controller
    {
        private readonly IDataRepositoy<TblRegLink, RegLinks> _dataRepository;

        public RegLinkController(IDataRepositoy<TblRegLink, RegLinks> dataRepositoy)
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
                TblRegLink tblRegLink = ((RegLinkDataManager)_dataRepository).GetByCode(code);
                if(tblRegLink == null)
                {
                    errorMsg = "Provided code does not exist in the system.";
                } else if (!tblRegLink.IsUsed)
                {   TblRegLink finalEntity = new TblRegLink() { UsedByEmail = email };

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