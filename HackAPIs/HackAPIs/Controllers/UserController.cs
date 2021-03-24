using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db.model;
using HackAPIs.Services.Db.Model;
using HackAPIs.Services.Teams;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Teams;
using HackAPIs.Services.Util;
using HackAPIs.ViewModel.Email;
using HackAPIs.ViewModel.Util;
using Newtonsoft.Json.Linq;
using HackAPIs.Model.Db;

namespace HackAPIs.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IDataRepositoy<tblUsers, Users> _dataRepository;
        private readonly IDataRepositoy<tblLog, Log> _dataRepositoryLog;

        private readonly IDataRepositoy<tblTeamHackers, TeamHackers> _teamHackersdataRepository;
        public UserController(IDataRepositoy<tblUsers, Users> dataRepositoy,
            IDataRepositoy<tblTeamHackers, TeamHackers> teamHackersdataRepository,
            IDataRepositoy<tblLog, Log> dataRepositoyLog)
        {
            _dataRepository = dataRepositoy;
            _dataRepositoryLog = dataRepositoyLog;
            _teamHackersdataRepository = teamHackersdataRepository;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult Get()
        {
            var tblUsers = _dataRepository.GetAll();
            return Ok(tblUsers);
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
                tblUsers oneUser = (tblUsers)usersEnumerator.Current;
                
                Boolean isExist = false;
                while (teamHackersEnumerator.MoveNext())
                {
                    tblTeamHackers oneTeamHacker = (tblTeamHackers)teamHackersEnumerator.Current;
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
            while (enumerator.MoveNext())
            {
                tblTeamHackers solution = (tblTeamHackers)enumerator.Current;

                SolutionList.Add(solution.TeamId);
            }
            userTeams.TeamId = SolutionList;

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
                tblUserSkillMatch skills = (tblUserSkillMatch)enumerator.Current;

                SkillsList.Add(skills.SkillId);
            }
            userSkillMatch.SkillId = SkillsList;

            return Ok(userSkillMatch);
        }

      

        // POST: api/users/regemail
        [HttpPost("regemail", Name = "GetUserByRegEmail")]
        public IActionResult GetUserByRegEmail([FromBody] tblUsers tblUsers)
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
        public IActionResult GetUserByMSEmail([FromBody] tblUsers tblUsers)
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
        public IActionResult Post([FromBody] tblUsers tblUsers)
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
        public async Task<IActionResult> Put(int id, [FromBody] tblUsers tblUsers)
        {
            int type = 1;
            string mailChimpId = "";

            if (tblUsers == null)
            {
                return BadRequest("User is null.");
            }

            var userToUpdate = _dataRepository.Get(id, 1);
            if (userToUpdate == null)
            {
                return NotFound("The User record couldn't be found.");
            }
            Log(id+"", "Record Exist");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!tblUsers.Active)
            {
                Log(id + "", "Inactivation of user is requested");
                TeamsService teamService = new TeamsService(_dataRepository);
                TeamMember member = new TeamMember();
                member.MemberID = userToUpdate.ADUserId;
                var usr = teamService.RemoveTeamMember(member);
                type = 4;



                if (tblUsers.UserRole.Contains("Hacker"))
                {
                    MailChimpService mailChimpService = new MailChimpService();
                    mailChimpId = await mailChimpService.InvokeMailChimp(tblUsers.UserMSTeamsEmail, tblUsers.UserDisplayName,
                        tblUsers.UserDisplayName, userToUpdate.MailchimpId, "unsubscribed", 2);
                    tblUsers.MailchimpId = mailChimpId;
                    Log(id + "", "MailChimp Unscribed request was completed for mailchinp ID: "+mailChimpId);

                }
            }
            else if (userToUpdate.ADUserId == null)
            {
                try
                {
                    Log(id + "", "Azure AD is Null");
                    TeamsService teamService = new TeamsService(_dataRepository);
                    GuestUser guest = new GuestUser();
                    guest.InvitedUserEmailAddress = tblUsers.UserMSTeamsEmail;
                    guest.UserId = userToUpdate.UserId;
                    guest = await teamService.InviteGuestUser(guest);
                    tblUsers.ADUserId = guest.ADUserId;
                    await teamService.UpdateMembers(guest.ADUserId, tblUsers.UserDisplayName);
                    Log(id + "", "Added Azure ID"+guest.ADUserId);

                    try
                    {
                        EmailService emailService = new EmailService();

                        emailService.InvokeEmail(tblUsers.UserMSTeamsEmail, tblUsers.UserDisplayName);
                        Log(id + "", "Email was sent to : "+ tblUsers.UserMSTeamsEmail);


                    } catch (Exception ex)
                    {

                    }

                    

                    // Add to MailChimp audience
                    if (tblUsers.UserRole.Contains("Hacker"))
                    {
                        MailChimpService mailChimpService = new MailChimpService();
                        mailChimpId = await mailChimpService.InvokeMailChimp(tblUsers.UserMSTeamsEmail, tblUsers.UserDisplayName.Substring(0,tblUsers.UserDisplayName.IndexOf(" ")),
                            tblUsers.UserDisplayName, tblUsers.MailchimpId, "subscribed", 1);
                        tblUsers.MailchimpId = mailChimpId;
                        Log(id + "", "Subscribed to MailChimp with ID: "+mailChimpId);

                    }
           
                }
                catch (Exception ex)
                {
                    
                }
            }
            _dataRepository.Update(userToUpdate, tblUsers, type);
            Log(id + "", "Completed the update of the record.");
            return Ok("Success");
        }


        // PUT: api/users/5
        [HttpPut("skills/{id}")]
        public IActionResult UserSkills(int id, [FromBody] tblUsers tblUsers)
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
        public IActionResult UserTeams(int id, [FromBody] tblUsers tblUsers)
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
            tblLog log = new tblLog
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
