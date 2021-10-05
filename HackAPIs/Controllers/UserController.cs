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
using HackAPIs.Services.Util;
using HackAPIs.Model.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataRepositoy<TblUsers, Users> _dataRepository;
        private readonly IDataRepositoy<TblLog, Log> _dataRepositoryLog;
        private readonly IDataRepositoy<TblTeamHackers, TeamHackers> _teamHackersdataRepository;
        private readonly IDataRepositoy<TblTeams, Solutions> _teamDataRepository;
        private readonly GitHubService _gitHubService;
        private readonly TeamsService _teamsService;
        private readonly TeamsServiceOptions _teamConfig;
        private readonly MailChimpService _mailChimp;

        public UserController(IDataRepositoy<TblUsers, Users> dataRepositoy,
            IDataRepositoy<TblTeamHackers, TeamHackers> teamHackersdataRepository,
            IDataRepositoy<TblTeams, Solutions> teamDataRepository,
            IDataRepositoy<TblLog, Log> dataRepositoyLog,
            GitHubService gitHubService,
            TeamsService teamsService,
            IOptions<TeamsServiceOptions> teamOptions,
            MailChimpService mailChimp)
        {
            _dataRepository = dataRepositoy;
            _dataRepositoryLog = dataRepositoyLog;
            _teamHackersdataRepository = teamHackersdataRepository;
            _teamDataRepository = teamDataRepository;
            _gitHubService = gitHubService;
            _teamsService = teamsService;
            _teamConfig = teamOptions.Value;
            _mailChimp = mailChimp;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult Get()
        {
            var tblUsers = _dataRepository.GetAll();
            return Ok(tblUsers);
        }

        [HttpGet("githubid/{id}")]
        public IActionResult GetGitHubId(long id)
        {
            var user = _dataRepository.Get(id, 1);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new TblUsers { GitHubId = user.GitHubId });
        }

        // GET: api/users
        [HttpGet("solutions/nousers", Name = "GetUserNoSolutions")]
        public IActionResult GetUserNoSolutions()
        {

            var tblUsers = _dataRepository.GetAll();

            IEnumerator usersEnumerator = tblUsers.GetEnumerator();

            var tblTeamHackers = _teamHackersdataRepository.GetAll();
            IEnumerator teamHackersEnumerator = tblTeamHackers.GetEnumerator();

            List<Users> users = new List<Users>();

            while (usersEnumerator.MoveNext())
            {
                TblUsers oneUser = (TblUsers)usersEnumerator.Current;

                Boolean isExist = false;
                while (teamHackersEnumerator.MoveNext())
                {
                    TblTeamHackers oneTeamHacker = (TblTeamHackers)teamHackersEnumerator.Current;
                    if (oneTeamHacker.UserId == oneUser.UserId)
                    {
                        isExist = true;
                    }
                }
                teamHackersEnumerator = tblTeamHackers.GetEnumerator();

                if (!isExist)
                {
                    Users user = new Users();
                    user.UserId = oneUser.UserId;
                    user.UserDisplayName = oneUser.UserDisplayName;
                    user.MySkills = oneUser.MySkills;
                    user.UserRole = oneUser.UserRole;
                    user.UserRegEmail = oneUser.UserRegEmail;
                    user.UserMSTeamsEmail = oneUser.UserMSTeamsEmail;
                    user.Active = oneUser.Active;
                    user.UserOptOut = user.UserOptOut;
                    if (user.Active)
                    {
                        users.Add(user);
                    }

                }

            }

            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var user = _dataRepository.Get(id, 1);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // GET: api/users/5
        [HttpGet("solutions/{id}", Name = "GetUserSolutions")]
        public IActionResult GetUserSolutions(long id)
        {
            var tblUsers = _dataRepository.Get(id, 2);
            if (tblUsers == null)
            {
                return NotFound("User not found.");
            }

            UserTeams userTeams = new UserTeams();
            userTeams.UserId = tblUsers.UserId;


            ArrayList SolutionList = new ArrayList();
            IEnumerator enumerator = tblUsers.tblTeamHackers.GetEnumerator();
            int islead = 0;
            while (enumerator.MoveNext())
            {
                TblTeamHackers solution = (TblTeamHackers)enumerator.Current;
                islead = solution.IsLead;
                SolutionList.Add(solution.TeamId);
            }
            userTeams.TeamId = SolutionList;
            userTeams.IsLead = islead;
            return Ok(userTeams);
        }

        // GET: api/users/5
        [HttpGet("skills/{id}", Name = "GetUserSkills")]
        public IActionResult GetUserSkills(long id)
        {
            var tblUsers = _dataRepository.Get(id, 3);
            if (tblUsers == null)
            {
                return NotFound("User not found.");
            }

            UserSkillMatch userSkillMatch = new UserSkillMatch();
            userSkillMatch.UserId = tblUsers.UserId;

            ArrayList SkillsList = new ArrayList();
            IEnumerator enumerator = tblUsers.tblUserSkillMatch.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TblUserSkillMatch skills = (TblUserSkillMatch)enumerator.Current;

                SkillsList.Add(skills.SkillId);
            }
            userSkillMatch.SkillId = SkillsList;

            return Ok(userSkillMatch);
        }



        // POST: api/users/regemail
        [HttpPost("regemail", Name = "GetUserByRegEmail")]
        public IActionResult GetUserByRegEmail([FromBody] TblUsers tblUsers)
        {
            var email = tblUsers.UserRegEmail;
            var user = _dataRepository.GetByColumn(1, "UserRegEmail", email);
            if (user == null)
            {
                return Ok(new ErrorObj());

            }

            return Ok(user);
        }

        // POST: api/users/msemail
        [HttpPost("msemail", Name = "GetUserByMSEmail")]
        public IActionResult GetUserByMSEmail([FromBody] TblUsers tblUsers)
        {
            var email = tblUsers.UserMSTeamsEmail;
            var user = _dataRepository.GetByColumn(1, "UserMSTeamsEmail", email);
            if (user == null)
            {
                return Ok(new ErrorObj());
            }

            return Ok(user);
        }


        // POST: api/users
        [HttpPost]
        public IActionResult Post([FromBody] TblUsers tblUsers)
        {
            if (tblUsers is null)
            {
                return BadRequest("User is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            tblUsers.CreatedDate = DateTime.Now;
            tblUsers.ModifiedDate = DateTime.Now;
            _dataRepository.Add(tblUsers);

            return Ok(tblUsers);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TblUsers tblUsers)
        {
            int type = 1;
            string mailChimpId;

            if (tblUsers == null)
            {
                return BadRequest("User is null.");
            }

            var userToUpdate = _dataRepository.Get(id, 1);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to validate incoming object.");
            }

            if (!tblUsers.Active)
            {
                try
                {
                    Log(id + "", "Inactivation of user is requested");
                    await _teamsService.RemoveTeamMember(_teamConfig.MSTeam1, userToUpdate.ADUserId);
                }
                catch (Exception) { }

                type = 4;

                try
                {
                    mailChimpId = await _mailChimp.AddOrUpdateMember(tblUsers.UserMSTeamsEmail, tblUsers.UserDisplayName, ViewModel.Util.MemberStatus.unsubscribed);
                    tblUsers.MailchimpId = mailChimpId;
                    Log(id + "", "MailChimp Unscribed request was completed for mailchinp ID: " + mailChimpId);
                }
                catch (Exception) { }
            }
            else if (userToUpdate.ADUserId == null)
            {
                try
                {
                    Log(id + "", "Azure AD is Null");
                    GuestUser guest = new GuestUser
                    {
                        InvitedUserEmailAddress = tblUsers.UserMSTeamsEmail,
                        UserId = userToUpdate.UserId,
                        DisplayName = userToUpdate.UserDisplayName
                    };
                    var invite = await _teamsService.InviteGuestUser(guest);
                    tblUsers.ADUserId = invite.InvitedUser.Id;
                    Log(id + "", "Added Azure ID" + tblUsers.ADUserId);

                    try
                    {
                        mailChimpId = await _mailChimp.AddOrUpdateMember(tblUsers.UserMSTeamsEmail, tblUsers.UserDisplayName, ViewModel.Util.MemberStatus.subscribed);

                        switch (tblUsers.UserRole.ToLower()) // TODO: add tags based on role
                        {
                            case "coach":
                                await _mailChimp.AddMemberTag(mailChimpId, "Coach - Fall 21");
                                break;
                            case "developer":
                                await _mailChimp.AddMemberTag(mailChimpId, "Developer - Fall 21");
                                break;
                            case "hacker":
                                await _mailChimp.AddMemberTag(mailChimpId, "Hacker - Fall 21");
                                break;
                            case "panelist":
                                await _mailChimp.AddMemberTag(mailChimpId, "Panelist - Fall 21");
                                break;
                            case "pitch coach":
                                await _mailChimp.AddMemberTag(mailChimpId, "Pitch Coach - Fall 21");
                                break;
                            default:
                                await _mailChimp.AddMemberTag(mailChimpId, "DevTest");
                                break;
                        }

                        Log(id + "", "Subscribed to MailChimp with ID: " + mailChimpId);

                    }
                    catch (Exception) { }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            _dataRepository.Update(userToUpdate, tblUsers, type);
            Log(id + "", "Completed the update of the record.");
            return Ok(tblUsers);
        }


        // PUT: api/users/5
        [HttpPut("skills/{id}")]
        public IActionResult UserSkills(int id, [FromBody] TblUsers tblUsers)
        {
            if (tblUsers == null)
            {
                return BadRequest("User is null.");
            }

            var userToUpdate = _dataRepository.Get(id, 1);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(userToUpdate, tblUsers, 2);
            return Ok("Success");
        }

        // PUT: api/users/5
        [HttpPut("solutions/{id}")]
        public async Task<IActionResult> UserTeams(int id, [FromBody] TblUsers tblUsers, [FromQuery] string teamName, [FromQuery] int isFromCreate)
        {
            if (tblUsers == null)
            {
                return BadRequest("User is null.");
            }

            var userToUpdate = _dataRepository.Get(id, 1);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(userToUpdate, tblUsers, 3);

            // Leaving the Team && determining whether this is being called from new team creation. This stops from calling GH api twice
            if (tblUsers.tblTeamHackers.Count != 0 && isFromCreate == 0)
            {
                try
                {
                    var team = _teamDataRepository.Get(tblUsers.tblTeamHackers.First().TeamId, 1);
                    await _gitHubService.AddUser(tblUsers.GitHubUser, tblUsers.GitHubId, team.GitHubTeamId);
                }
                catch (GitHubException gex)
                {
                    return BadRequest(gex);
                }
            }

            return Ok("Success");
        }

        [HttpPut("github/{id}")]
        public IActionResult GitHubUserId(int id, [FromBody] TblUsers tblUsers)
        {
            if (tblUsers == null)
            {
                return BadRequest("User is null.");
            }

            var userToUpdate = _dataRepository.Get(id, 1);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(userToUpdate, tblUsers, 5);

            return Ok("Success");
        }

        /*
       // GET: api/users/5
       [HttpGet("{id}", Name = "GetUser")]
       public IActionResult Get(int id)
       {
           var user = _dataRepository.GetDto(id);
           if (user == null)
           {
               return NotFound("User not found.");
           }

           return Ok(user);
       }
       */

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
