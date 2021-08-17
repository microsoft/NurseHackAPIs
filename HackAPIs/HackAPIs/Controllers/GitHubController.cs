using HackAPIs.Db.Model;
using HackAPIs.Model.Db.Repository;
using HackAPIs.ViewModel.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route ("/api/GitHub")]
    [ApiController]
    public class GitHubController : Controller
    {
        private readonly IDataRepositoy<tblTeams, Solutions> _dataRepository;

        private readonly IDataRepositoy<tblTeamHackers, TeamHackers> _teamHackersdataRepository;
        public GitHubController(IDataRepositoy<tblTeams, Solutions> dataRepositoy,
            IDataRepositoy<tblTeamHackers, TeamHackers> teamHackersdataRepository)
        {
            _dataRepository = dataRepositoy;
            _teamHackersdataRepository = teamHackersdataRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
