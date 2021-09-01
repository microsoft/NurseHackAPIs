using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Db.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HackAPIs.Model.Db.DataManager
{
    public class SurveyDataManager : IDataRepositoy<TblSurvey, Survey>
    {
        readonly NurseHackContext _nurseHackContext;

        public SurveyDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(TblSurvey entity)
        {
            _nurseHackContext.tbl_Survey.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Delete(TblSurvey entity)
        {
            throw new NotImplementedException();
        }

        public TblSurvey Get(long id, int type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TblSurvey> GetAll()
        {
            throw new NotImplementedException();
        }

        public TblSurvey GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new NotImplementedException();
        }

        public TblSurvey GetByObject(TblSurvey entity)
        {
            throw new NotImplementedException();
        }

        public Survey GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(TblSurvey entityToUpdate, TblSurvey entity, int type)
        {
            throw new NotImplementedException();
        }
    }
}
