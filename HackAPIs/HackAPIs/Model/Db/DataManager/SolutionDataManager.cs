﻿using Microsoft.EntityFrameworkCore;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Services.Db.model;
using HackAPIs.Services.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackAPIs.Model.Db.DataManager
{
    public class SolutionDataManager : IDataRepositoy<tblTeams, Solutions>
    {
        readonly NurseHackContext _nurseHackContext;

        public SolutionDataManager(NurseHackContext nurseHackContext)
        {
            _nurseHackContext = nurseHackContext;
        }

        public IEnumerable<tblTeams> GetAll()
        {
            return _nurseHackContext.tbl_Teams
                .ToList();
        }

        public tblTeams Get(long id,int type)
        {
            _nurseHackContext.ChangeTracker.LazyLoadingEnabled = false;
            tblTeams tblTeams = null;
            if (type == 1)
            {
                tblTeams = _nurseHackContext.tbl_Teams
                    .SingleOrDefault(b => b.TeamId == id);
            }
            else if (type == 2)
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
            else if (type == 3)
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
            } else if (type ==4)
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

        
        public void Add(tblTeams entity)
        {
            _nurseHackContext.tbl_Teams.Add(entity);
            _nurseHackContext.SaveChanges();
        }

        public void Update(tblTeams entityToUpdate, tblTeams entity, int type)
        {
            if (type == 1)
            {
                entityToUpdate = _nurseHackContext.tbl_Teams
                    .Single(b => b.TeamId == entityToUpdate.TeamId);

                entityToUpdate.TeamName = entity.TeamName;
                entityToUpdate.TeamDescription = entity.TeamDescription;
                entityToUpdate.GithubURL = entity.GithubURL;
                entityToUpdate.MSTeamsChannel = entity.MSTeamsChannel;
                entityToUpdate.MSLabEnvironment = entity.MSLabEnvironment;
                entityToUpdate.MSLabTenantName = entity.MSLabTenantName;
                entityToUpdate.MSLabAzureUsername = entity.MSLabAzureUsername;
                entityToUpdate.MSLabSPNAppId = entity.MSLabSPNAppId;
                entityToUpdate.MSLabSPNAppObjectId = entity.MSLabSPNAppObjectId;
                entityToUpdate.MSLabSPNObjectId = entity.MSLabSPNObjectId;
                entityToUpdate.MSLabSPNDisplayName = entity.MSLabSPNDisplayName;
                entityToUpdate.MSLabSPNKey = entity.MSLabSPNKey;
                entityToUpdate.Active = entity.Active;
                entityToUpdate.ChallengeName = entity.ChallengeName;
                entityToUpdate.SkillsWanted = entity.SkillsWanted;
            }
            else if (type == 2)
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
                    catch (Exception ex)
                    {

                    }

                }



            }
            else if (type == 3)
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
                    catch (Exception ex)
                    {

                    }
                }
            }
            _nurseHackContext.SaveChanges();

        }

        public void Delete(tblTeams entity)
        {
            throw new System.NotImplementedException();
        }

        public tblTeams GetByColumn(long id, string columnName, string colunmValue)
        {
            throw new System.NotImplementedException();
        }

        public tblTeams GetByObject(tblTeams entity)
        {
            throw new NotImplementedException();
        }
    }
}