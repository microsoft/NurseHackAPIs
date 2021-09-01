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
    public class RegLinkDataManager : IDataRepositoy<TblRegLink, RegLinks>
    {
        readonly NurseHackContext _nurseHackContext;

        public RegLinkDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public TblRegLink GetAll()
        {
                        throw new NotImplementedException();
        }

        public TblRegLink Get(long id, int type)
        {
            TblRegLink tblReg = null;
            tblReg = _nurseHackContext.tbl_RegLink
                .SingleOrDefault(b => b.RegLinkId == id);
           
            return tblReg;
        }

        public TblRegLink GetByCode(Guid uniqueCode)
        {
            TblRegLink tblReg = null;
            tblReg = _nurseHackContext.tbl_RegLink
                .SingleOrDefault(b => b.UniqueCode == uniqueCode);

            return tblReg;
        }

        public TblRegLink GetByColumn(long id, string columnName, string columnValue)
        {
            TblRegLink tblReg = null;
            if (columnName.Equals("UniqueCode"))
            {
                tblReg = _nurseHackContext.tbl_RegLink
                    .SingleOrDefault(b => b.UniqueCode.ToString() == columnValue);
            }

            return tblReg;
        }     

        IEnumerable<TblRegLink> IDataRepositoy<TblRegLink, RegLinks>.GetAll()
        {
            throw new NotImplementedException();
        }

        TblRegLink IDataRepositoy<TblRegLink, RegLinks>.GetByObject(TblRegLink entity)
        {
            throw new NotImplementedException();
        }

        RegLinks IDataRepositoy<TblRegLink, RegLinks>.GetDto(long id)
        {
            throw new NotImplementedException();
        }

        void IDataRepositoy<TblRegLink, RegLinks>.Add(TblRegLink entity)
        {
            throw new NotImplementedException();
        }

        void IDataRepositoy<TblRegLink, RegLinks>.Update(TblRegLink entityToUpdate, TblRegLink entity, int type)
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

        void IDataRepositoy<TblRegLink, RegLinks>.Delete(TblRegLink entity)
        {
            throw new NotImplementedException();
        }
    }
}
