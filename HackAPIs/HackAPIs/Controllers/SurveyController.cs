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
    [Route("api/survey")]
    [ApiController]
    public class SurveyController : Controller
    {
        private readonly IDataRepositoy<tblSurvey, Survey> _dataRepository;

        public SurveyController(IDataRepositoy<tblSurvey, Survey> dataRepositoy)
        {
            _dataRepository = dataRepositoy;
        }
        // GET: api/Survey
        [HttpGet]
        public IActionResult Get()
        {
            var tblSkills = _dataRepository.GetAll();
            return Ok(tblSkills);
        }

        // GET: api/survey/5
        [HttpGet("{id}", Name = "GetSurvey")]
        public IActionResult Get(int id)
        {
            var skill = _dataRepository.GetDto(id);
            if (skill == null)
            {
                return NotFound("Survey not found.");
            }

            return Ok(skill);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Survey survey)
        {
            if (survey is null)
            {
                return BadRequest("Survey is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            tblSurvey tblSurvey = new tblSurvey()
            {
                 UserId = survey.UserId,
                 Company = survey.Company,
                 Country = survey.Country,
                 Expertise = survey.Expertise,
                 Pronouns = survey.Pronouns,
                 RaceEthnicity = survey.RaceEthnicity,
                 State = survey.State,
                 Student = survey.Student
                 
            };

            _dataRepository.Add(tblSurvey);
            return Ok(survey);
        }

        
    }
}
