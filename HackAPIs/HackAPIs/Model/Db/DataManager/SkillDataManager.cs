using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Services.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Db.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class SkillDataManager : IDataRepositoy<tblSkills, Skills>
    {
        readonly NurseHackContext _nurseHackContext;

        public SkillDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<tblSkills> GetAll()
        {
            return _nurseHackContext.tbl_Skills
                .ToList();
        }

        public tblSkills Get(long id, int type)
        {
            var tblSkills = _nurseHackContext.tbl_Skills
                .SingleOrDefault(b => b.SkillId == id);

            return tblSkills;
        }

        public Skills GetDto(long id)
        {
            _nurseHackContext.ChangeTracker.LazyLoadingEnabled = true;

           
                var skills = _nurseHackContext.tbl_Skills
                    .SingleOrDefault(b => b.SkillId == id);

                return SkillsDTOMapper.MapToDto(skills);
            
        }

        public void Add(tblSkills entity)
        {
            _nurseHackContext.tbl_Skills.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(tblSkills entityToUpdate, tblSkills entity, int type)
        {
            entityToUpdate = _nurseHackContext.tbl_Skills
                .Single(b => b.SkillId == entityToUpdate.SkillId);

            entityToUpdate.SkillName = entity.SkillName;

            _nurseHackContext.SaveChanges();

        }

        public void Delete(tblSkills entity)
        {
            throw new System.NotImplementedException();
        }

  

        public tblSkills GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new System.NotImplementedException();
        }

        public tblSkills GetByObject(tblSkills entity)
        {
            throw new NotImplementedException();
        }
    }
}
