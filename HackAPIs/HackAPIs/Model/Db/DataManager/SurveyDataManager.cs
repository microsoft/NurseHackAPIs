using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Services.Db.Model;
using HackAPIs.ViewModel.Db;
using HackAPIs.ViewModel.Db.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HackAPIs.Model.Db.DataManager
{
    public class SurveyDataManager : IDataRepositoy<tblSurvey, Survey>
    {
        readonly NurseHackContext _nurseHackContext;

        public SurveyDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(tblSurvey entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(tblSurvey entity)
        {
            throw new NotImplementedException();
        }

        public tblSurvey Get(long id, int type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblSurvey> GetAll()
        {
            throw new NotImplementedException();
        }

        public tblSurvey GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new NotImplementedException();
        }

        public tblSurvey GetByObject(tblSurvey entity)
        {
            throw new NotImplementedException();
        }

        public Survey GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(tblSurvey entityToUpdate, tblSurvey entity, int type)
        {
            throw new NotImplementedException();
        }
    }
}
