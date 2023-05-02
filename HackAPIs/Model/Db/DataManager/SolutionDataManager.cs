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
    public class SolutionDataManager : IDataRepositoy<TblTeams, Solutions>
    {
        readonly NurseHackContext _nurseHackContext;

        public SolutionDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<TblTeams> GetAll()
        {
            return _nurseHackContext.tbl_Teams
                .ToList();
        }

        public TblTeams Get(long id, ExtendedDataType extendedData)
        {
            _nurseHackContext.ChangeTracker.LazyLoadingEnabled = false;
            TblTeams tblTeams = null;
            if (extendedData == ExtendedDataType.BaseOnly)
            {
                tblTeams = _nurseHackContext.tbl_Teams
                    .SingleOrDefault(b => b.TeamId == id);
            }
            else if (extendedData == ExtendedDataType.Solutions)
            {
                tblTeams = _nurseHackContext.tbl_Teams
                .SingleOrDefault(b => b.TeamId == id);

                if (tblTeams == null)
                {
                    return null;
                }
                _nurseHackContext.Entry(tblTeams)
                    .Collection(b => b.tblTeamHackers)
                    .Load();
            } 
            else if (extendedData == ExtendedDataType.Skills)
            {
                tblTeams = _nurseHackContext.tbl_Teams
                .SingleOrDefault(b => b.TeamId == id);

                if (tblTeams == null)
                {
                    return null;
                }
                _nurseHackContext.Entry(tblTeams)
                    .Collection(b => b.tblTeamSkillMatch)
                    .Load();
            } else if (extendedData == ExtendedDataType.UpdateADId)
            {
                tblTeams = _nurseHackContext.tbl_Teams
               .SingleOrDefault(b => b.TeamId == id);

                if (tblTeams == null)
                {
                    return null;
                }
                _nurseHackContext.Entry(tblTeams)
                    .Collection(b => b.tblTeamHackers)
                    .Load();

                _nurseHackContext.Entry(tblTeams)
                    .Collection(b => b.tblTeamSkillMatch)
                    .Load();

            }
           

            return tblTeams;
        }

        public Solutions GetDto(long id)
        {
            throw new System.NotImplementedException();
        }

        
        public void Add(TblTeams entity)
        {
            _nurseHackContext.tbl_Teams.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(TblTeams entityToUpdate, TblTeams entity, ExtendedDataType extendedData)
        {
            /*
             * Type 1 = PUT /solutions/$teamid (modify team)
             * We are only updating description. Ignore all other fields
             */
            if (extendedData == ExtendedDataType.BaseOnly)
            {
                entityToUpdate = _nurseHackContext.tbl_Teams
                    .Single(b => b.TeamId == entityToUpdate.TeamId);
                entityToUpdate.TeamName = entity.TeamName;
                entityToUpdate.TeamDescription = entity.TeamDescription;
                entityToUpdate.ModifiedDate = entity.ModifiedDate;
                entityToUpdate.Active = entity.Active;
                entityToUpdate.ChallengeName = entity.ChallengeName;
                entityToUpdate.SkillsWanted = entity.SkillsWanted;

                //entityToUpdate.GithubURL = entity.GithubURL;
                //entityToUpdate.MSTeamsChannelName = entity.MSTeamsChannelName;
                //entityToUpdate.MSTeamsChannelUrl = entity.MSTeamsChannelUrl;
                //entityToUpdate.MSLabEnvironment = entity.MSLabEnvironment;
                //entityToUpdate.MSLabTenantName = entity.MSLabTenantName;
                //entityToUpdate.MSLabAzureUsername = entity.MSLabAzureUsername;
                //entityToUpdate.MSLabSPNAppId = entity.MSLabSPNAppId;
                //entityToUpdate.MSLabSPNAppObjectId = entity.MSLabSPNAppObjectId;
                //entityToUpdate.MSLabSPNObjectId = entity.MSLabSPNObjectId;
                //entityToUpdate.MSLabSPNDisplayName = entity.MSLabSPNDisplayName;
                //entityToUpdate.MSLabSPNKey = entity.MSLabSPNKey;
                //entityToUpdate.ModifiedBy = entity.ModifiedBy;
            }
            else if (extendedData == ExtendedDataType.Solutions)
            {
                entityToUpdate = _nurseHackContext.tbl_Teams
                    .Include(a => a.tblTeamSkillMatch)
                    .Single(b => b.TeamId == entityToUpdate.TeamId);

                var deletedSkills = entityToUpdate.tblTeamSkillMatch.Except(entity.tblTeamSkillMatch).ToList();
                var addedSkills = entity.tblTeamSkillMatch.Except(entityToUpdate.tblTeamSkillMatch).ToList();

                deletedSkills.ForEach(skillToDelete =>
                    entityToUpdate.tblTeamSkillMatch.Remove(
                        entityToUpdate.tblTeamSkillMatch
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
            else if (extendedData == ExtendedDataType.Skills)
            {
                entityToUpdate = _nurseHackContext.tbl_Teams
                    .Include(a => a.tblTeamHackers)
                    .Single(b => b.TeamId == entityToUpdate.TeamId);

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
                    }
                    catch (Exception) { }
                }
            }
            _nurseHackContext.SaveChanges();
        }

        public void Delete(TblTeams entity)
        {
            throw new System.NotImplementedException();
        }

        public TblTeams GetByColumn(long id, string columnName, string columnValue)
        {
            throw new System.NotImplementedException();
        }

        public TblTeams GetByObject(TblTeams entity)
        {
            throw new NotImplementedException();
        }
    }
}