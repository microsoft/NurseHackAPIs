using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HackAPIs.Services.Db.model;
using HackAPIs.Services.Db.Model;

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
       
        public DbSet<tblSkills> tbl_Skills { get; set; }
        public DbSet<tblUserSkillMatch> tbl_UserSkillMatch { get; set; }
        public DbSet<tblTeamHackers> tbl_TeamHackers { get; set; }
        public DbSet<tblTeams> tbl_Teams { get; set; }
        public DbSet<tblTeamSkillMatch> tbl_TeamSkillMatch { get; set; }
        public DbSet<tblUsers> tbl_Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblTeamSkillMatch>().HasKey(ts => new { ts.TeamId, ts.SkillId });
            modelBuilder.Entity<tblUserSkillMatch>().HasKey(us => new { us.UserId, us.SkillId });
            modelBuilder.Entity<tblTeamHackers>().HasKey(ts => new { ts.TeamId, ts.UserId });

        }
    }
}
