using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Db.Model;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using HackAPIs.ViewModel.Util;
using HackAPIs.Services.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using HackAPIs.Model.Db;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/solutions")]
    [ApiController]
    [Authorize]
    public class SolutionController : ControllerBase
    {
        private readonly IDataRepositoy<TblTeams, Solutions> _dataRepository;
        private readonly IDataRepositoy<TblSkills, Skills> _skilldataRepository;
        private readonly IDataRepositoy<TblUsers, Users> _userDataRepository;
        private readonly IDataRepositoy<TblLog, Log> _dataRepositoryLog;
        private readonly GitHubService _githubService;
        private readonly TeamsService _teamsService;
        private readonly TeamsServiceOptions _teamConfig;

        public SolutionController(IDataRepositoy<TblTeams, Solutions> dataRepositoy,
         IDataRepositoy<TblSkills, Skills> skilldataRepositoy,
         IDataRepositoy<TblUsers, Users> userDataRepository,
         IDataRepositoy<TblLog, Log> dataRepositoyLog,
         GitHubService gitHubService,
         TeamsService teamsService,
         IOptions<TeamsServiceOptions> teamOptions)
        {
            _dataRepository = dataRepositoy;
            _skilldataRepository = skilldataRepositoy;
            _userDataRepository = userDataRepository;
            _dataRepositoryLog = dataRepositoyLog;
            _githubService = gitHubService;
            _teamsService = teamsService;
            _teamConfig = teamOptions.Value;
        }

        // GET: api/solutions
        [HttpGet]
        public IActionResult Get()
        {
            var tblTeams = _dataRepository.GetAll()
                .Where(a => a.Active);

            return Ok(tblTeams);
        }

        // GET: api/solutions/5
        [HttpGet("{id}", Name = "GetSolution")]
        public IActionResult Get(int id)
        {
            var tblTeams = _dataRepository.Get(id, 1);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

            return Ok(tblTeams);
        }

        // GET: api/solutions/hackers
        [HttpGet("hackers", Name = "GetSolutionsHackers")]
        public IActionResult GetSolutionsHackers()
        {
            var tblTeams = _dataRepository.GetAll();

            IEnumerator teamEnumerator = tblTeams.GetEnumerator();
            List<SolutionHackers> solutionList = new List<SolutionHackers>();

            while (teamEnumerator.MoveNext())
            {
                TblTeams tblTeam = (TblTeams)teamEnumerator.Current;
                TblTeams oneTeam = _dataRepository.Get(tblTeam.TeamId, 2);

                SolutionHackers solutionHackers = new SolutionHackers();
                solutionHackers.TeamId = oneTeam.TeamId;
                solutionHackers.TeamName = oneTeam.TeamName;

                ArrayList HackerList = new ArrayList();
                List<HackerExpanded> hackers = new List<HackerExpanded>();
                IEnumerator enumerator = oneTeam.tblTeamHackers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TblTeamHackers hacker = (TblTeamHackers)enumerator.Current;
                    TblUsers user = _userDataRepository.Get(hacker.UserId, 1);
                    hackers.Add(new HackerExpanded() { name = user.UserDisplayName, islead = hacker.IsLead });
                    HackerList.Add(hacker.UserId);
                }
                solutionHackers.UserID = HackerList;
                solutionHackers.Hackers = hackers;
                solutionList.Add(solutionHackers);

            }

            //  return Ok(tblTeams);
            return Ok(solutionList);
        }

        // GET: api/solutions/hackers/5
        [HttpGet("hackers/{id}", Name = "GetSolutionHackers")]
        public IActionResult GetSolutionHackers(long id)
        {
            var tblTeams = _dataRepository.Get(id, 2);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

            SolutionHackers solutionHackers = new SolutionHackers();
            solutionHackers.TeamId = tblTeams.TeamId;
            solutionHackers.TeamName = tblTeams.TeamName;
            List<HackerExpanded> hackers = new List<HackerExpanded>();
            ArrayList HackerList = new ArrayList();
            IEnumerator enumerator = tblTeams.tblTeamHackers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TblTeamHackers hacker = (TblTeamHackers)enumerator.Current;
                HackerList.Add(hacker.UserId);
                TblUsers user = (TblUsers)_userDataRepository.Get(hacker.UserId, 1);
                HackerExpanded thisHacker = new HackerExpanded() { name = user.UserDisplayName, islead = hacker.IsLead };
                hackers.Add(thisHacker);
            }
            solutionHackers.UserID = HackerList;
            solutionHackers.Hackers = hackers;

            return Ok(solutionHackers);
        }

        // GET: api/solutions/skills/5
        [HttpGet("skills/{id}", Name = "GetSolutionSkills")]
        public IActionResult GetSolutionSkills(long id)
        {
            var tblTeams = _dataRepository.Get(id, 3);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

            SolutionSkills solutionSkills = new SolutionSkills();
            solutionSkills.TeamId = tblTeams.TeamId;
            solutionSkills.TeamName = tblTeams.TeamName;

            ArrayList SkillsList = new ArrayList();
            IEnumerator enumerator = tblTeams.tblTeamSkillMatch.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TblTeamSkillMatch skills = (TblTeamSkillMatch)enumerator.Current;

                SkillsList.Add(skills.SkillId);
            }
            solutionSkills.SkillId = SkillsList;

            return Ok(solutionSkills);
        }


        // GET: api/solutions/skills/5
        [HttpGet("hackers/skills/{id}", Name = "GetSolutionHackersSkills")]
        public IActionResult GetSolutionHackersSkills(long id)
        {
            var tblTeams = _dataRepository.Get(id, 4);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

            SolutionHackersSkills SolutionHackersSkills = new SolutionHackersSkills();
            SolutionHackersSkills.TeamId = tblTeams.TeamId;
            SolutionHackersSkills.TeamName = tblTeams.TeamName;
            ArrayList SkillsList = new ArrayList();
            IEnumerator enumerator = tblTeams.tblTeamSkillMatch.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TblTeamSkillMatch skills = (TblTeamSkillMatch)enumerator.Current;

                Skills SkillsDTO = _skilldataRepository.GetDto(skills.SkillId);
                //    SkillsList.Add(skills.SkillId);
                SkillsList.Add(SkillsDTO);
            }
            SolutionHackersSkills.SkillId = SkillsList;

            ArrayList HackerList = new ArrayList();
            enumerator = tblTeams.tblTeamHackers.GetEnumerator();

            while (enumerator.MoveNext())
            {
                TblTeamHackers hacker = (TblTeamHackers)enumerator.Current;
                HackerList.Add(hacker.UserId);
            }
            SolutionHackersSkills.UserID = HackerList;

            return Ok(SolutionHackersSkills);
        }


        // POST: api/solutions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TblTeams tblTeams)
        {
            if (tblTeams is null)
            {
                return BadRequest("Solution is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Feature flag in appsettings Teams:ChannelAutoCreate = true|false
            if (_teamConfig.ChannelAutoCreate)
            {
                var teamName = string.IsNullOrEmpty(tblTeams.ChallengeName) ?
                    tblTeams.TeamName :
                    string.Concat(tblTeams.ChallengeName, " - ", tblTeams.TeamName);

                var channel = await _teamsService.CreateTeamChannel(_teamConfig.MSTeam1, teamName, tblTeams.TeamDescription);
                tblTeams.MSTeamsChannelUrl = channel.WebUrl;
                tblTeams.MSTeamsChannelName = channel.DisplayName;
            }

            (int TeamId, long RepoId) = await _githubService.CreateRepoAndTeam(tblTeams.TeamName, tblTeams.TeamDescription);

            tblTeams.GitHubTeamId = TeamId;
            tblTeams.GitHubRepoId = (int)RepoId;
            tblTeams.Active = true;
            tblTeams.CreatedDate = DateTime.Now;

            _dataRepository.Add(tblTeams);

            await AddUserToGHTeam(tblTeams.CreatedBy, TeamId);

            return Ok(tblTeams);
        }

        // PUT: api/solutions/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id, 1);
            if (solutionToUpdate == null)
            {
                return NotFound("The Solution record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            tblTeams.ModifiedDate = DateTime.Now;
            
            tblTeams.Active = true;
            _dataRepository.Update(solutionToUpdate, tblTeams, 1);
            return Ok("Success");
        }


        // PUT: api/solutions/skills/5
        [HttpPut("skills/{id}")]
        public IActionResult SolutionSkills(int id, [FromBody] TblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id, 1);
            if (solutionToUpdate == null)
            {
                return NotFound("The Solution record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(solutionToUpdate, tblTeams, 2);
            return Ok("Success");
        }

        // PUT: api/solutions/users/5
        [HttpPut("hackers/{id}")]
        public IActionResult SolutionUsers(int id, [FromBody] TblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id, 1);
            if (solutionToUpdate == null)
            {
                return NotFound("The Solution record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(solutionToUpdate, tblTeams, 3);
            return Ok("Success");
        }

        private async Task AddUserToGHTeam(string createdBy, int teamId)
        {
            try
            {
                var user = _userDataRepository.GetByColumn(1, "UserRegEmail", createdBy);
                await _githubService.AddUser(user.GitHubUser, user.GitHubId, teamId);
            }
            catch (Exception ex)
            {
                Log(teamId.ToString(), ex.Message);
            }
        }

        private void Log(string id, string type)
        {
            TblLog log = new TblLog
            {
                Label = id,
                Description = type,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            _dataRepositoryLog.Add(log);
        }
    }
}