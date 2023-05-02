using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class LogDataManager : IDataRepositoy<TblLog, Log>
    {
        readonly NurseHackContext _nurseHackContext;

        public LogDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(TblLog entity)
        {
            _nurseHackContext.tbl_Log.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Delete(TblLog entity)
        {
            throw new NotImplementedException();
        }

        public TblLog Get(long id, ExtendedDataType extendedData)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TblLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public TblLog GetByColumn(long id, string columnName, string columnValue)
        {
            throw new NotImplementedException();
        }

        public TblLog GetByObject(TblLog entity)
        {
            throw new NotImplementedException();
        }

        public Log GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(TblLog entityToUpdate, TblLog entity, ExtendedDataType extendedData)
        {
            throw new NotImplementedException();
        }
    }
}
