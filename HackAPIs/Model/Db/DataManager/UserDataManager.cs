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

        public TblUsers Get(long id, ExtendedDataType extendedData)
        {
            TblUsers tblUsers = null;
            if (extendedData == ExtendedDataType.BaseOnly)
            {
                tblUsers = _nurseHackContext.tbl_Users
                    .SingleOrDefault(b => b.UserId == id);
            }
            else if (extendedData == ExtendedDataType.Skills) // User and Skills
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
            else if (extendedData == ExtendedDataType.Solutions) // User and Solutions
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

        public void Update(TblUsers entityFromDB, TblUsers entityFromRequest, ExtendedDataType extendedData)
        {
            /**
             * Type 2 = PUT /user/skills/$userid (editing user's skills)
             * Type 3 = PUT /user/solution/$userid (adding/removing solution teams from a user)
             * Type 4 = PUT /user/github/$userid (add's a user's github id and username to the user object)
             */
            if (extendedData == ExtendedDataType.BaseOnly || extendedData == ExtendedDataType.UpdateADId)
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityFromDB.UserId);

                if (!string.IsNullOrEmpty(entityFromRequest.UserDisplayName)) entityFromDB.UserDisplayName = entityFromRequest.UserDisplayName;
                if (!string.IsNullOrEmpty(entityFromRequest.UserMSTeamsEmail)) entityFromDB.UserMSTeamsEmail = entityFromRequest.UserMSTeamsEmail;
                if (!string.IsNullOrEmpty(entityFromRequest.UserRegEmail)) entityFromDB.UserRegEmail = entityFromRequest.UserRegEmail;
                if (!string.IsNullOrEmpty(entityFromRequest.UserRole)) entityFromDB.UserRole = entityFromRequest.UserRole;
                if (!string.IsNullOrEmpty(entityFromRequest.UserTimeCommitment)) entityFromDB.UserTimeCommitment = entityFromRequest.UserTimeCommitment;
                if (!string.IsNullOrEmpty(entityFromRequest.MailchimpId)) entityFromDB.MailchimpId = entityFromRequest.MailchimpId;
                entityFromDB.Active = entityFromRequest.Active;
                entityFromDB.MSFTOptIn = entityFromRequest.MSFTOptIn;
                entityFromDB.JNJOptIn = entityFromRequest.JNJOptIn;
                entityFromDB.SONSIELOptIn = entityFromRequest.SONSIELOptIn;


                if (extendedData == ExtendedDataType.BaseOnly) // Updating the new Azure AD ID from the Azure AD
                {
                    if (!string.IsNullOrEmpty(entityFromRequest.ADUserId)) entityFromDB.ADUserId = entityFromRequest.ADUserId;
                }
                else if (extendedData == ExtendedDataType.UpdateADId) // Updating the existing Azure AD ID from the Azure AD
                {
                    entityFromDB.ADUserId = entityFromRequest.ADUserId;
                }
                entityFromDB.UserOptOut = entityFromRequest.UserOptOut;
                if (!string.IsNullOrEmpty(entityFromRequest.MySkills)) entityFromDB.MySkills = entityFromRequest.MySkills;
                entityFromDB.ModifiedDate = DateTime.Now;
            }
            else if (extendedData == ExtendedDataType.Skills) // User Skills
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Include(a => a.tblUserSkillMatch)
                    .Single(b => b.UserId == entityFromDB.UserId);

                var deletedSkills = entityFromDB.tblUserSkillMatch.Except(entityFromRequest.tblUserSkillMatch).ToList();
                var addedSkills = entityFromRequest.tblUserSkillMatch.Except(entityFromDB.tblUserSkillMatch).ToList();

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
            else if (extendedData == ExtendedDataType.Solutions) // User Solutions
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Include(a => a.tblTeamHackers)
                    .Single(b => b.UserId == entityFromDB.UserId);

                var deletedTeams = entityFromDB.tblTeamHackers.Except(entityFromRequest.tblTeamHackers).ToList();
                var addedTeams = entityFromRequest.tblTeamHackers.Except(entityFromDB.tblTeamHackers).ToList();

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
            else if (extendedData == ExtendedDataType.GithubId) // Github id
            {
                entityFromDB = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityFromDB.UserId);
                entityFromDB.GitHubId = entityFromRequest.GitHubId;
                entityFromDB.GitHubUser = entityFromRequest.GitHubUser;
            }
            _nurseHackContext.SaveChanges();

        }

        public void Delete(TblUsers entity)
        {
            throw new NotImplementedException();
        }
    }
}
