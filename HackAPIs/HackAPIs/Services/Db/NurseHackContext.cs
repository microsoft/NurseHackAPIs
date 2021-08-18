using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HackAPIs.Db.Model;
using HackAPIs.Model.Db;

namespace HackAPIs.Services.Db
{
    public class NurseHackContext : DbContext
    {
        public NurseHackContext()
        {
        }

        public NurseHackContext(DbContextOptions<NurseHackContext> options) : base(options)
        {
           
        }
       
        public DbSet<TblSkills> tbl_Skills { get; set; }
        public DbSet<TblUserSkillMatch> tbl_UserSkillMatch { get; set; }
        public DbSet<TblTeamHackers> tbl_TeamHackers { get; set; }
        public DbSet<TblTeams> tbl_Teams { get; set; }
        public DbSet<TblTeamSkillMatch> tbl_TeamSkillMatch { get; set; }
        public DbSet<TblUsers> tbl_Users { get; set; }
        public DbSet<TblLog> tbl_Log { get; set; }
        public DbSet<TblSurvey> tbl_Survey { get; set; }
        public DbSet<TblRegLink> tbl_RegLink { get;   set;  }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblTeamSkillMatch>().HasKey(ts => new { ts.TeamId, ts.SkillId });
            modelBuilder.Entity<TblUserSkillMatch>().HasKey(us => new { us.UserId, us.SkillId });
            modelBuilder.Entity<TblTeamHackers>().HasKey(ts => new { ts.TeamId, ts.UserId });

        }
    }
}
