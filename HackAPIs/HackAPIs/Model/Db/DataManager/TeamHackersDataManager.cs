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
    public class TeamHackersDataManager : IDataRepositoy<tblTeamHackers, TeamHackers>
    {
        readonly NurseHackContext _nurseHackContext;

        public TeamHackersDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public void Add(tblTeamHackers entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(tblTeamHackers entity)
        {
            throw new NotImplementedException();
        }

        public tblTeamHackers Get(long id, int type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblTeamHackers> GetAll()
        {
            return _nurseHackContext.tbl_TeamHackers
                .ToList();
        }

        public tblTeamHackers GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new NotImplementedException();
        }

        public tblTeamHackers GetByObject(tblTeamHackers tblTeamHackers)
        {
            _nurseHackContext.Entry(tblTeamHackers)
                   .Reload();
            return tblTeamHackers;
        }

        public TeamHackers GetDto(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(tblTeamHackers entityToUpdate, tblTeamHackers entity, int type)
        {
            throw new NotImplementedException();
        }
    }

   
}
