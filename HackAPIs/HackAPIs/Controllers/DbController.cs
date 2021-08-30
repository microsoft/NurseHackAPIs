using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HackAPIs.Services.Db;
using HackAPIs.ViewModel.Teams;
using Microsoft.AspNetCore.Authorization;

namespace HackAPIs.Controllers
{
    [Authorize]
    //Controller
    public class DbController : Controller
    {
     //   private readonly ILogger<TeamsController> _logger;
        private readonly NurseHackContext _dbContext;

        public DbController(NurseHackContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("solutions")]
        public async Task<string> GetSolutions()
        {
            DbService dbService = new DbService();
            return await dbService.GetTeamSolutions();

        }

        [HttpGet("members/{solutionID}")]
        public async Task<string> GetSolutionMember(string solutionID)
        {
            DbService dbService = new DbService();
            return await dbService.GetSolutionMembers(solutionID);

        }

        [HttpPost("member/{SolutionMember}")]
        public async Task<string> AddMember(TeamMember member)
        {
            DbService dbService = new DbService();
            return await Task.FromResult("");
     //       return await dbService.GetSolutionMember(solutionID);

        }

        [HttpDelete("member/{SolutionMember}")]
        public async Task<string> RemoveMember(TeamMember member)
        {
            DbService dbService = new DbService();
            return await Task.FromResult("");
            //     return await dbService.RemoveSolutionMember(solutionID);

        }
    }
}
