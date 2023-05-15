using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
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
using HackAPIs.Services.Teams;
using Azure.Identity;
using Microsoft.Extensions.Options;

namespace HackAPIs 
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.Configure<GitHubServiceOptions>(Configuration.GetSection("GitHub"));
            services.AddSingleton<GitHubClient>(o =>
            {
                var client = new GitHubClient(new ProductHeaderValue("nh4h-apis"));
                client.Credentials = new Credentials(Configuration["GitHubToken"]);
                return client;
            });
            services.AddSingleton<GitHubService>();

            services.Configure<TeamsServiceOptions>(Configuration.GetSection("Teams"));

            // Register a custom configuration provider to convert the comma-delimited setting to a List<string>
            services.AddSingleton<IConfigureOptions<TeamsServiceOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new ConfigureFromCommaSeparatedString<TeamsServiceOptions>(
                    configuration.GetSection("Teams:TeamIds"));
            });



            services.AddScoped<TeamsService>(t =>
            {
                var graph = Configuration.GetSection("GraphAPI");                
                var tenantId = graph["TenantId"];
                var clientId = graph["ClientId"];
                var secret = graph["ClientSecret"];
                var scope = graph["Scope"];

                var creds = new ClientSecretCredential(tenantId, clientId, secret);
                var client = new GraphServiceClient(creds, new[] { scope });

                var options = new TeamsServiceOptions();
                Configuration.GetSection("Teams").Bind(options);
                return new TeamsService(options, client);
            });

            services.Configure<MailChimpOptions>(Configuration.GetSection("MailChimp"));
            services.AddHttpClient<MailChimpService>();
            
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