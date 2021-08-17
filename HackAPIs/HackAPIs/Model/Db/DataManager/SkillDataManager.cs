using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Db.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class SkillDataManager : IDataRepositoy<TblSkills, Skills>
    {
        readonly NurseHackContext _nurseHackContext;

        public SkillDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<TblSkills> GetAll()
        {
            return _nurseHackContext.tbl_Skills
                .ToList();
        }

        public TblSkills Get(long id, int type)
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

        public void Add(TblSkills entity)
        {
            _nurseHackContext.tbl_Skills.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(TblSkills entityToUpdate, TblSkills entity, int type)
        {
            entityToUpdate = _nurseHackContext.tbl_Skills
                .Single(b => b.SkillId == entityToUpdate.SkillId);

            entityToUpdate.SkillName = entity.SkillName;

            _nurseHackContext.SaveChanges();

        }

        public void Delete(TblSkills entity)
        {
            throw new System.NotImplementedException();
        }

  

        public TblSkills GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new System.NotImplementedException();
        }

        public TblSkills GetByObject(TblSkills entity)
        {
            throw new NotImplementedException();
        }
    }
}
