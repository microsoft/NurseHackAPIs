using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class RegLinkDataManager : IDataRepositoy<tblRegLink, RegLinks>
    {
        readonly NurseHackContext _nurseHackContext;

        public RegLinkDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public tblRegLink GetAll()
        {
                        throw new NotImplementedException();
        }

        public tblRegLink Get(long id, int type)
        {
            tblRegLink tblReg = null;
            tblReg = _nurseHackContext.tbl_RegLink
                .SingleOrDefault(b => b.RegLinkId == id);
           
            return tblReg;
        }

        public tblRegLink GetByCode(Guid uniqueCode)
        {
            tblRegLink tblReg = null;
            tblReg = _nurseHackContext.tbl_RegLink
                .SingleOrDefault(b => b.UniqueCode == uniqueCode);

            return tblReg;
        }

        public tblRegLink GetByColumn(long id, string columnName, string columnValue)
        {
            tblRegLink tblReg = null;
            if (columnName.Equals("UniqueCode"))
            {
                tblReg = _nurseHackContext.tbl_RegLink
                    .SingleOrDefault(b => b.UniqueCode.ToString() == columnValue);
            }

            return tblReg;
        }     

        IEnumerable<tblRegLink> IDataRepositoy<tblRegLink, RegLinks>.GetAll()
        {
            throw new NotImplementedException();
        }

        tblRegLink IDataRepositoy<tblRegLink, RegLinks>.GetByObject(tblRegLink entity)
        {
            throw new NotImplementedException();
        }

        RegLinks IDataRepositoy<tblRegLink, RegLinks>.GetDto(long id)
        {
            throw new NotImplementedException();
        }

        void IDataRepositoy<tblRegLink, RegLinks>.Add(tblRegLink entity)
        {
            throw new NotImplementedException();
        }

        void IDataRepositoy<tblRegLink, RegLinks>.Update(tblRegLink entityToUpdate, tblRegLink entity, int type)
        {
            if (entity.UsedByEmail != null)
            {
                entityToUpdate = _nurseHackContext.tbl_RegLink
                        .Single(b => b.RegLinkId == entityToUpdate.RegLinkId);
                entityToUpdate.IsUsed = true;

                entityToUpdate.UsedByEmail = entity.UsedByEmail;


            }

            _nurseHackContext.SaveChanges();
        }

        void IDataRepositoy<tblRegLink, RegLinks>.Delete(tblRegLink entity)
        {
            throw new NotImplementedException();
        }
    }
}
