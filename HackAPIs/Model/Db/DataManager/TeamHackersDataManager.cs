using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class TeamHackersDataManager : IDataRepositoy<TblTeamHackers, TeamHackers>
    {
        readonly NurseHackContext _nurseHackContext;

        public TeamHackersDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(TblTeamHackers entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TblTeamHackers entity)
        {
            throw new NotImplementedException();
        }

        public TblTeamHackers Get(long id, ExtendedDataType extendedData)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TblTeamHackers> GetAll()
        {
            return _nurseHackContext.tbl_TeamHackers
                .ToList();
        }

        public TblTeamHackers GetByColumn(long id, string columnName, string columnValue)
        {
            throw new NotImplementedException();
        }

        public TblTeamHackers GetByObject(TblTeamHackers tblTeamHackers)
        {
            _nurseHackContext.Entry(tblTeamHackers)
                   .Reload();
            return tblTeamHackers;
        }

        public TeamHackers GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(TblTeamHackers entityToUpdate, TblTeamHackers entity, ExtendedDataType extendedData)
        {
            throw new NotImplementedException();
        }
    }

   
}
