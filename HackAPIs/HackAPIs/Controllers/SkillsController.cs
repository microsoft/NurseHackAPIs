using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using Microsoft.AspNetCore.Authorization;

namespace HackAPIs.Controllers
{
    [Authorize]
    [Route("api/skills")]
    [ApiController]
    [Authorize]
    public class SkillsController : Controller
    {
        private readonly IDataRepositoy<TblSkills, Skills> _dataRepository;

        public SkillsController(IDataRepositoy<TblSkills,Skills> dataRepositoy)
        {
            _dataRepository = dataRepositoy;
        }

        // GET: api/Skills
        [HttpGet]
        public IActionResult Get()
        {
            var tblSkills = _dataRepository.GetAll();
            return Ok(tblSkills);
        }

        // GET: api/Skills/5
        [HttpGet("{id}", Name = "GetSkill")]
        public IActionResult Get(int id)
        {
            var skill = _dataRepository.GetDto(id);
            if (skill == null)
            {
                return NotFound("Skill not found.");
            }

            return Ok(skill);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Skills skills)
        {
            if (skills is null)
            {
                return BadRequest("Skills is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TblSkills tblSkills = new TblSkills()
            {
                SkillId = skills.SkillId,
                SkillName = skills.SkillName
            };

            _dataRepository.Add(tblSkills);
            return Ok(skills);
        }

        // PUT: api/Skills/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Skills skills)
        {
            if (skills == null)
            {
                return BadRequest("Skill is null.");
            }

            var skillToUpdate = _dataRepository.Get(id,1);
            if (skillToUpdate == null)
            {
                return NotFound("The Skill record couldn't be found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TblSkills tblSkills = new TblSkills()
            {
                SkillId = skills.SkillId,
                SkillName = skills.SkillName
            };

            _dataRepository.Update(skillToUpdate, tblSkills,1);
            return Ok(skills);
        }
    }
}

