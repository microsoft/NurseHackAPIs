using HackAPIs.Model.Db;
using HackAPIs.Model.Db.Repository;
using HackAPIs.ViewModel.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Controllers
{
    [Route ("/api/GitHub")]
    [ApiController]
    public class GitHubController : Controller
    {
        private readonly IDataRepositoy<tblTeams, Solutions> _dataRepository;
        //private readonly IDataRepositoy<tblLog, Log> _dataRepositoryLog;

        private readonly IDataRepositoy<tblTeamHackers, TeamHackers> _teamHackersdataRepository;
        public UserController(IDataRepositoy<tblUsers, Users> dataRepositoy,
            IDataRepositoy<tblTeamHackers, TeamHackers> teamHackersdataRepository,
            IDataRepositoy<tblLog, Log> dataRepositoyLog)
        {
            _dataRepository = dataRepositoy;
            _dataRepositoryLog = dataRepositoyLog;
            _teamHackersdataRepository = teamHackersdataRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
