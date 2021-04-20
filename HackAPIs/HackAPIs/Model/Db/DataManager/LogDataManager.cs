using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class LogDataManager : IDataRepositoy<tblLog, Log>
    {
        readonly NurseHackContext _nurseHackContext;

        public LogDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(tblLog entity)
        {
            _nurseHackContext.tbl_Log.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Delete(tblLog entity)
        {
            throw new NotImplementedException();
        }

        public tblLog Get(long id, int type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public tblLog GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new NotImplementedException();
        }

        public tblLog GetByObject(tblLog entity)
        {
            throw new NotImplementedException();
        }

        public Log GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(tblLog entityToUpdate, tblLog entity, int type)
        {
            throw new NotImplementedException();
        }
    }
}
