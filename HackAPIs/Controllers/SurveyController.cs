﻿using HackAPIs.Model.Db;
using HackAPIs.Model.Db.Repository;
using HackAPIs.ViewModel.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/survey")]
    [ApiController]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly IDataRepositoy<TblSurvey, Survey> _dataRepository;

        public SurveyController(IDataRepositoy<TblSurvey, Survey> dataRepositoy)
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

            TblSurvey tblSurvey = new TblSurvey()
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
