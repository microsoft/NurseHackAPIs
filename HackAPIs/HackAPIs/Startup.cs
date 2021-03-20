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
using HackAPIs.Services.Db.model;
using HackAPIs.Services.Db.Model;
using HackAPIs.ViewModel.Db;
using System;
using HackAPIs.Model.Db;
using HackAPIs.Services.Util;

namespace HackAPIs 
{
    public class Startup
    {

        private string ConnStr = null;
        private string ClientTeamEmbed = null;
        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //  services.AddControllers();
            ConnStr = Configuration["ConnStr"];
            ClientTeamEmbed = Configuration["ClientTeamEmbed"];
            UtilConst.SMTPFromAddress = Configuration["EmailFromAddress"];
            UtilConst.SMTP = Configuration["EmailSMTPAddress"];
            UtilConst.SMTPPassword = Configuration["svc-NH4H-devupconf-org"];
            UtilConst.SMTPUser = Configuration["SMTPUser"]; 
            UtilConst.StorageConn = Configuration["EmailTemplateStorage"];
            UtilConst.Container = Configuration["EmailTemplateContainer"];
            UtilConst.Blob = Configuration["EmailTemplateBlob"];


            services.AddDbContext<NurseHackContext>(options =>
                options.UseSqlServer(ConnStr)
                .EnableSensitiveDataLogging());
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                builder =>
                                {
                                    builder.WithOrigins("*")
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                                });
            });

            services.AddScoped<IDataRepositoy<tblSkills, Skills>,SkillDataManager>();
            services.AddScoped<IDataRepositoy<tblUsers, Users>, UserDataManager>();
            services.AddScoped<IDataRepositoy<tblTeams, Solutions>, SolutionDataManager>();
            services.AddScoped<IDataRepositoy<tblTeamHackers, TeamHackers>, TeamHackersDataManager>();
            services.AddScoped<IDataRepositoy<tblLog, Log>, LogDataManager>();
            services.AddScoped<IDataRepositoy<tblSurvey, Survey>, SurveyDataManager>();
            services.AddScoped<IDataRepositoy<tblRegLink, RegLinks>, RegLinkDataManager>();

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

            app.Use(async (context, next) =>
            {
                //    var headerList = context.Request.Headers;
                IHeaderDictionary headerList = context.Request.Headers;

                // Options

                if (context.Request.Method == "OPTIONS")
                {

                    context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, ClientTeamEmbed" });
                    context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                    context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                    context.Response.Headers.Add("ClientTeamEmbed", new[] { ClientTeamEmbed });


                }
                //Options


                var headerKeys = headerList.Keys;

                foreach (var key in headerKeys)
                {
                    //   context.Response.Headers.Add(key, headerList[key].ToString());
                }

                Boolean header = context.Request.Headers.ContainsKey("ClientTeamEmbed");
                var val = context.Request.Headers["ClientTeamEmbed"].ToString();



                if (!header || (!val.Equals(ClientTeamEmbed)))
                {

                    if (!val.Contains(ClientTeamEmbed))
                    {
                        context.Response.WriteAsync("Security validation failed. The API access is denied!");
                    }

                }
                else
                {

                    await next();
                }

            });

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

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}