using Microsoft.EntityFrameworkCore;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HackAPIs.Model.Db.DataManager
{
    public class UserDataManager : IDataRepositoy<TblUsers, Users>
    {
        readonly NurseHackContext _nurseHackContext;

        public UserDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<TblUsers> GetAll()
        {
            return _nurseHackContext.tbl_Users
                .ToList();
        }

        public TblUsers Get(long id, int type)
        {
            TblUsers tblUsers = null;
            if (type == 1)
            {
                tblUsers = _nurseHackContext.tbl_Users
                    .SingleOrDefault(b => b.UserId == id);
            }
            else if (type == 3) // User and Skills
            {
                tblUsers = _nurseHackContext.tbl_Users
                   .SingleOrDefault(b => b.UserId == id);
                if (tblUsers == null)
                {
                    return null;
                }
                _nurseHackContext.Entry(tblUsers)
                    .Collection(b => b.tblUserSkillMatch)
                    .Load();
            }
            else if (type == 2) // User and Solutions
            {
                tblUsers = _nurseHackContext.tbl_Users
                  .SingleOrDefault(b => b.UserId == id);
                if (tblUsers == null)
                {
                    return null;
                }
                _nurseHackContext.Entry(tblUsers)
                    .Collection(b => b.tblTeamHackers)
                    .Load();
            }
            return tblUsers;
        }

        public TblUsers GetByObject(TblUsers tblUsers)
        {
            if (tblUsers == null)
            {
                return null;
            }
            _nurseHackContext.Entry(tblUsers)
                .Collection(b => b.tblTeamHackers)
                .Load();

            return tblUsers;
        }

        public TblUsers GetByColumn(long id, string columnName, string columnValue)
        {
            TblUsers tblUsers = null;
            if (columnName.Equals("UserMSTeamsEmail"))
            {
                //  .SingleOrDefault(b => b.UserMSTeamsEmail == columnValue);

                tblUsers = _nurseHackContext.tbl_Users
                           .SingleOrDefault(b => b.UserMSTeamsEmail == columnValue);
            }
            else if (columnName.Equals("UserRegEmail"))
            {
                tblUsers = _nurseHackContext.tbl_Users
                        .FirstOrDefault(b => b.UserRegEmail == columnValue);
            }
            else if (columnName.Equals("ADUserId"))
            {
                tblUsers = _nurseHackContext.tbl_Users
                    .FirstOrDefault(b => b.ADUserId == columnValue);
            }

            return tblUsers;
        }

        public Users GetDto(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(TblUsers entity)
        {
            _nurseHackContext.tbl_Users.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(TblUsers entityFromDB, TblUsers passedInEntity, int type)
        {
            /**
             * Type 2 = PUT /user/skills/$userid (editing user's skills)
             * Type 3 = PUT /user/solution/$userid (adding/removing solution teams from a user)
             * Type 4 = PUT /user/github/$userid (add's a user's github id and username to the user object)
             */
            if (type == 0)
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityFromDB.UserId);
                entityFromDB.ADUserId = passedInEntity.ADUserId;
            }
            if (type == 1 || type == 4)
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityFromDB.UserId);

                if (!string.IsNullOrEmpty(passedInEntity.UserDisplayName)) entityFromDB.UserDisplayName = passedInEntity.UserDisplayName;
                if (!string.IsNullOrEmpty(passedInEntity.UserMSTeamsEmail)) entityFromDB.UserMSTeamsEmail = passedInEntity.UserMSTeamsEmail;
                if (!string.IsNullOrEmpty(passedInEntity.UserRegEmail)) entityFromDB.UserRegEmail = passedInEntity.UserRegEmail;
                if (!string.IsNullOrEmpty(passedInEntity.UserRole)) entityFromDB.UserRole = passedInEntity.UserRole;
                if (!string.IsNullOrEmpty(passedInEntity.UserTimeCommitment)) entityFromDB.UserTimeCommitment = passedInEntity.UserTimeCommitment;
                if (!string.IsNullOrEmpty(passedInEntity.MailchimpId)) entityFromDB.MailchimpId = passedInEntity.MailchimpId;
                entityFromDB.Active = passedInEntity.Active;
                entityFromDB.MSFTOptIn = passedInEntity.MSFTOptIn;
                entityFromDB.JNJOptIn = passedInEntity.JNJOptIn;
                entityFromDB.SONSIELOptIn = passedInEntity.SONSIELOptIn;


                if (type == 1) // Updating the new Azure AD ID from the Azure AD
                {
                    if (!string.IsNullOrEmpty(passedInEntity.ADUserId)) entityFromDB.ADUserId = passedInEntity.ADUserId;
                }
                else if (type == 4) // Updating the existing Azure AD ID from the Azure AD
                {
                    entityFromDB.ADUserId = passedInEntity.ADUserId;
                }
                entityFromDB.UserOptOut = passedInEntity.UserOptOut;
                if (!string.IsNullOrEmpty(passedInEntity.MySkills)) entityFromDB.MySkills = passedInEntity.MySkills;
                entityFromDB.ModifiedDate = DateTime.Now;
            }
            else if (type == 2) // User Skills
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Include(a => a.tblUserSkillMatch)
                    .Single(b => b.UserId == entityFromDB.UserId);

                var deletedSkills = entityFromDB.tblUserSkillMatch.Except(passedInEntity.tblUserSkillMatch).ToList();
                var addedSkills = passedInEntity.tblUserSkillMatch.Except(entityFromDB.tblUserSkillMatch).ToList();

                deletedSkills.ForEach(skillToDelete =>
                    entityFromDB.tblUserSkillMatch.Remove(
                        entityFromDB.tblUserSkillMatch
                            .First(b => b.SkillId == skillToDelete.SkillId)));

                foreach (var addedSkill in addedSkills)
                {
                    try
                    {
                        _nurseHackContext.Entry(addedSkill).State = EntityState.Added;
                    }
                    catch (Exception) { }
                }

            }
            else if (type == 3) // User Solutions
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Include(a => a.tblTeamHackers)
                    .Single(b => b.UserId == entityFromDB.UserId);

                var deletedTeams = entityFromDB.tblTeamHackers.Except(passedInEntity.tblTeamHackers).ToList();
                var addedTeams = passedInEntity.tblTeamHackers.Except(entityFromDB.tblTeamHackers).ToList();

                deletedTeams.ForEach(teamToDelete =>
                    entityFromDB.tblTeamHackers.Remove(
                        entityFromDB.tblTeamHackers
                            .First(b => b.TeamId == teamToDelete.TeamId)));

                foreach (var addedTeam in addedTeams)
                {
                    try
                    {
                        _nurseHackContext.Entry(addedTeam).State = EntityState.Added;
                    }
                    catch (Exception) { }
                }
            }
            else if (type == 5) // Github id
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityFromDB.UserId);
                entityFromDB.GitHubId = passedInEntity.GitHubId;
                entityFromDB.GitHubUser = passedInEntity.GitHubUser;
            }
            _nurseHackContext.SaveChanges();

        }

        public void Delete(TblUsers entity)
        {
            throw new NotImplementedException();
        }
    }
}
