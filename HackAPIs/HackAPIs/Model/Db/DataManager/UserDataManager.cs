using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HackAPIs.Model.Db.DataManager
{
    public class UserDataManager : IDataRepositoy<tblUsers,Users>
    {
        readonly NurseHackContext _nurseHackContext;

        public UserDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<tblUsers> GetAll()
        {
            return _nurseHackContext.tbl_Users
                .ToList();
        }

        public tblUsers Get(long id, int type)
        {
            tblUsers tblUsers = null;
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

        public tblUsers GetByObject(tblUsers tblUsers)
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

        public tblUsers GetByColumn(long id, string columnName,string columnValue)
        {
            tblUsers tblUsers = null;
            if (columnName.Equals("UserMSTeamsEmail"))
            {
                //  .SingleOrDefault(b => b.UserMSTeamsEmail == columnValue);
               
                tblUsers = _nurseHackContext.tbl_Users
                           .SingleOrDefault(b => b.UserMSTeamsEmail == columnValue);
            } else if (columnName.Equals("UserRegEmail"))
            {
                tblUsers = _nurseHackContext.tbl_Users
                        .FirstOrDefault(b => b.UserRegEmail == columnValue);
            }

            return tblUsers;
        }

        public Users GetDto(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(tblUsers entity)
        {
            _nurseHackContext.tbl_Users.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(tblUsers entityToUpdate, tblUsers entity, int type)
        {
            /**
             * Type 2 = PUT /user/skills/$userid (editing user's skills)
             * Type 3 = PUT /user/solution/$userid (adding/removing solution teams from a user)
             * Type 4 = PUT /user/github/$userid (add's a user's github id and username to the user object)
             */
            if (type == 0)
            {
                entityToUpdate = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityToUpdate.UserId);
                entityToUpdate.ADUserId = entity.ADUserId;
            }
            if (type == 1 || type == 4)
            {
                entityToUpdate = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityToUpdate.UserId);

                if (entity.UserDisplayName != null)
                    entityToUpdate.UserDisplayName = entity.UserDisplayName;
                if (entity.UserMSTeamsEmail != null)
                    entityToUpdate.UserMSTeamsEmail = entity.UserMSTeamsEmail;
                if (entity.UserRegEmail != null)
                    entityToUpdate.UserRegEmail = entity.UserRegEmail;
                if (entity.UserRole != null)
                    entityToUpdate.UserRole = entity.UserRole;
                if (entity.UserTimeCommitment != null)
                    entityToUpdate.UserTimeCommitment = entity.UserTimeCommitment;
                if (entity.Active != null)
                    entityToUpdate.Active = entity.Active;
                if (entity.MSFTOptIn != null)
                    entityToUpdate.MSFTOptIn = entity.MSFTOptIn;
                if (entity.JNJOptIn != null)
                    entityToUpdate.JNJOptIn = entity.JNJOptIn;
                if (entity.SONSIELOptIn != null)
                    entityToUpdate.SONSIELOptIn = entity.SONSIELOptIn;
                if (entity.MailchimpId != null)
                    entityToUpdate.MailchimpId = entity.MailchimpId;


                if (type == 1) // Updating the new Azure AD ID from the Azure AD
                {
                    if (entity.ADUserId != null)
                        entityToUpdate.ADUserId = entity.ADUserId;
                } else if (type == 4) // Updating the existing Azure AD ID from the Azure AD
                {
                    entityToUpdate.ADUserId = entity.ADUserId;
                }
                if (entity.UserOptOut != null)
                    entityToUpdate.UserOptOut = entity.UserOptOut;
                if (entity.MySkills != null)
                    entityToUpdate.MySkills = entity.MySkills;
                entityToUpdate.ModifiedDate = DateTime.Now;


            }
            else if (type == 2) // User Skills
            {
                entityToUpdate = _nurseHackContext.tbl_Users
                    .Include(a => a.tblUserSkillMatch)
                    .Single(b => b.UserId == entityToUpdate.UserId);



                var deletedSkills = entityToUpdate.tblUserSkillMatch.Except(entity.tblUserSkillMatch).ToList();
                var addedSkills = entity.tblUserSkillMatch.Except(entityToUpdate.tblUserSkillMatch).ToList();

                deletedSkills.ForEach(skillToDelete =>
                    entityToUpdate.tblUserSkillMatch.Remove(
                        entityToUpdate.tblUserSkillMatch
                            .First(b => b.SkillId == skillToDelete.SkillId)));

                foreach (var addedSkill in addedSkills)
                {
                    try
                    {
                        _nurseHackContext.Entry(addedSkill).State = EntityState.Added;
                    }
                    catch (Exception ex)
                    {

                    }

                }

            }
            else if (type == 3) // User Solutions
            {
                entityToUpdate = _nurseHackContext.tbl_Users
                    .Include(a => a.tblTeamHackers)
                    .Single(b => b.UserId == entityToUpdate.UserId);

                var deletedTeams = entityToUpdate.tblTeamHackers.Except(entity.tblTeamHackers).ToList();
                var addedTeams = entity.tblTeamHackers.Except(entityToUpdate.tblTeamHackers).ToList();

                deletedTeams.ForEach(teamToDelete =>
                    entityToUpdate.tblTeamHackers.Remove(
                        entityToUpdate.tblTeamHackers
                            .First(b => b.TeamId == teamToDelete.TeamId)));

                foreach (var addedTeam in addedTeams)
                {
                    try
                    {
                        _nurseHackContext.Entry(addedTeam).State = EntityState.Added;
                    } catch (Exception ex)
                    {

                    }
                }
            }
            else if (type == 5) // Github id
            {
                entityToUpdate = _nurseHackContext.tbl_Users
                    .Single(b => b.UserId == entityToUpdate.UserId);
                entityToUpdate.GitHubId = entity.GitHubId;
                entityToUpdate.GitHubUser = entity.GitHubUser;
            }
                _nurseHackContext.SaveChanges();
                    
        }

        public void Delete(tblUsers entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
