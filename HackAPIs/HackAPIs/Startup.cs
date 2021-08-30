using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HackAPIs.Model.Db.DataManager;
using HackAPIs.Model.Db.Repository;
using HackAPIs.Services.Db;
using HackAPIs.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using HackAPIs.Model.Db;
using HackAPIs.Services.Util;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Octokit;

namespace HackAPIs 
{
    public class Startup
    {

        private string ConnStr = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.Configure<GitHubServiceOptions>(Configuration.GetSection("GitHub"));
            services.AddSingleton<GitHubClient>(o =>
            {
                var client = new GitHubClient(new ProductHeaderValue("nh4h-apis"));
                client.Credentials = new Credentials(Configuration["GitHubToken"]);
                return client;
            });
            services.AddSingleton<GitHubService>();

            // TODO: This is literally the worst :(
            UtilConst.SMTPFromAddress = Configuration["EmailFromAddress"];
            UtilConst.SMTP = Configuration["EmailSMTPAddress"];
            UtilConst.SMTPPassword = Configuration["svc-NH4H-devupconf-org"];
            UtilConst.SMTPUser = Configuration["SMTPUser"]; 
            UtilConst.StorageConn = Configuration["EmailTemplateStorage"];
            UtilConst.Container = Configuration["EmailTemplateContainer"];
            UtilConst.Blob = Configuration["EmailTemplateBlob"];
            UtilConst.MSTeam1 = Configuration["MSTeam1"];
            UtilConst.MSTeam2 = Configuration["MSTeam2"];
            UtilConst.MailChimpKey = Configuration["MailChimpKey"];
            UtilConst.MailChimpURL = Configuration["MailChimpURL"];
            UtilConst.MailChimpAudience = Configuration["MailChimpAudience"];
            UtilConst.MailChimpUser = Configuration["MailChimpUser"];
            UtilConst.ClientId = Configuration["ClientId"];
            UtilConst.ClientSecret = Configuration["ClientSecret"];
            UtilConst.Tenant = Configuration["Tenant"];
            UtilConst.TeamDomain = Configuration["TeamDomain"];
            UtilConst.GitHubToken = Configuration["GitHubToken"];
            // end worst code ever

            services.AddDbContext<NurseHackContext>(options =>
                options.UseSqlServer(Configuration["ConnStr"]));

            services.AddScoped<IDataRepositoy<TblSkills, Skills>,SkillDataManager>();
            services.AddScoped<IDataRepositoy<TblUsers, Users>, UserDataManager>();
            services.AddScoped<IDataRepositoy<TblTeams, Solutions>, SolutionDataManager>();
            services.AddScoped<IDataRepositoy<TblTeamHackers, TeamHackers>, TeamHackersDataManager>();
            services.AddScoped<IDataRepositoy<TblLog, Log>, LogDataManager>();
            services.AddScoped<IDataRepositoy<TblSurvey, Survey>, SurveyDataManager>();
            services.AddScoped<IDataRepositoy<TblRegLink, RegLinks>, RegLinkDataManager>();

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}