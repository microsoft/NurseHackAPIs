using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db.model;
using HackAPIs.Services.Db.Model;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;

namespace HackAPIs.Controllers
{
    [Route("api/solutions")]
    [ApiController]
    public class SolutionController : Controller
    {
        private readonly IDataRepositoy<tblTeams, Solutions> _dataRepository;

        private readonly IDataRepositoy<tblSkills, Skills> _skilldataRepository;
       
        public SolutionController(IDataRepositoy<tblTeams, Solutions> dataRepositoy,
            IDataRepositoy<tblSkills, Skills> skilldataRepositoy)
        {
            _dataRepository = dataRepositoy;
            _skilldataRepository = skilldataRepositoy;
        }
        // GET: api/solutions
        [HttpGet]
        public IActionResult Get()
        {
            var tblTeams = _dataRepository.GetAll();


              return Ok(tblTeams);
            
        }


        // GET: api/solutions
        [HttpGet("hackers", Name = "GetSolutionsHackers")]
        public IActionResult GetSolutionsHackers()
        {
            var tblTeams = _dataRepository.GetAll();

            IEnumerator teamEnumerator = tblTeams.GetEnumerator();
            List<SolutionHackers> solutionList = new List<SolutionHackers>();

            while (teamEnumerator.MoveNext())
            {
                tblTeams tblTeam = (tblTeams)teamEnumerator.Current;
                tblTeams oneTeam = _dataRepository.Get(tblTeam.TeamId, 2);

                SolutionHackers solutionHackers = new SolutionHackers();
                solutionHackers.TeamId = oneTeam.TeamId;
                solutionHackers.TeamName = oneTeam.TeamName;

                ArrayList HackerList = new ArrayList();
                IEnumerator enumerator = oneTeam.tblTeamHackers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    tblTeamHackers hacker = (tblTeamHackers)enumerator.Current;

                    HackerList.Add(hacker.UserId);
                }
                solutionHackers.UserID = HackerList;

                solutionList.Add(solutionHackers);

            }

            //  return Ok(tblTeams);
            return Ok(solutionList);
        }

        // GET: api/solutions/5
        [HttpGet("{id}", Name = "GetSolution")]
        public IActionResult Get(int id)
        {
            var tblTeams = _dataRepository.Get(id,1);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

           

            return Ok(tblTeams);
        }

        // GET: api/solutions/5
        [HttpGet("hackers/{id}", Name = "GetSolutionHackers")]
        public IActionResult GetSolutionHackers(long id)
        {
            var tblTeams = _dataRepository.Get(id,2);
            if (tblTeams == null)
            {
                return NotFound("Solution not found.");
            }

            SolutionHackers solutionHackers = new SolutionHackers();
            solutionHackers.TeamId = tblTeams.TeamId;
            solutionHackers.TeamName = tblTeams.TeamName;

            ArrayList HackerList = new ArrayList();
            IEnumerator enumerator = tblTeams.tblTeamHackers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                tblTeamHackers hacker = (tblTeamHackers)enumerator.Current;

                HackerList.Add(hacker.UserId);
            }
            solutionHackers.UserID = HackerList;

            

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
                tblTeamSkillMatch skills = (tblTeamSkillMatch)enumerator.Current;

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
                tblTeamSkillMatch skills = (tblTeamSkillMatch)enumerator.Current;

                Skills SkillsDTO = _skilldataRepository.GetDto(skills.SkillId);
                //    SkillsList.Add(skills.SkillId);
                SkillsList.Add(SkillsDTO);
            }
            SolutionHackersSkills.SkillId = SkillsList;

            ArrayList HackerList = new ArrayList();
            enumerator = tblTeams.tblTeamHackers.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                tblTeamHackers hacker = (tblTeamHackers)enumerator.Current;
                HackerList.Add(hacker.UserId);
            }
            SolutionHackersSkills.UserID = HackerList;



            return Ok(SolutionHackersSkills);
        }


        // POST: api/solutions
        [HttpPost]
        public async Task<IActionResult>  Post([FromBody] tblTeams tblTeams)
        {
            if (tblTeams is null)
            {
                return BadRequest("Solution is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            ///todo - Make this work from a Keyvault flag.  CreateChannel T/F
            /*
                        var teampre = "Team-";
                        tblTeams.TeamName = teampre + tblTeams.TeamName;                    

                        try
                        {
                            TeamsService teamsService = new TeamsService();
                            TeamChannel teamChannel = new TeamChannel();
                            teamChannel.ChannelName = tblTeams.TeamName;
                            teamChannel.ChannelDescription = tblTeams.TeamDescription;
                            teamChannel = await teamsService.CreateTeamChannel(teamChannel);
                            tblTeams.MSTeamsChannel = teamChannel.ChannelWebURL;
                        } catch (Exception ex)
                        {

            }
            */
            _dataRepository.Add(tblTeams);

            return Ok(tblTeams);
        }

        // PUT: api/solutions/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] tblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id,1);
            if (solutionToUpdate == null)
            {
                return NotFound("The Solution record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            tblTeams.ModifiedDate = DateTime.Now;
            _dataRepository.Update(solutionToUpdate, tblTeams, 1);
            return Ok("Success");
        }


        // PUT: api/solutions/skills/5
        [HttpPut("skills/{id}")]
        public IActionResult SolutionSkills(int id, [FromBody] tblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id,1);
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
        public IActionResult SolutionUsers(int id, [FromBody] tblTeams tblTeams)
        {
            if (tblTeams == null)
            {
                return BadRequest("Solution is null.");
            }

            var solutionToUpdate = _dataRepository.Get(id,1);
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

    }
}