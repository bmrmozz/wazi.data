using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using wazi.data.core.master;
using wazi.data.models;

namespace wazidataservicetest
{
    public class Startup {
        IHostingEnvironment environment = null;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            this.environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development")) {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }



        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddOptions();

            services.Configure<MasterRepository>(masterRepository => {
                setupMasterRepository(masterRepository);
            });

            services.AddSingleton<IConfiguration>(Configuration);


            //services.AddSwaggerGen();

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //Add the detail information for the API.
            //services.ConfigureSwaggerGen(options => {
            //    options.SingleApiVersion(new Info {
            //        Version = "1.0.0",
            //        Title = "rcos repository service",
            //        Description = "Provides all information related to repositories stored and available to requesting entities.",
            //        Contact = new Swashbuckle.Swagger.Model.Contact { Name = "Brandon Roberts", Email = "bmrmozz@outlook.com", Url = "https://rcosonline.com/" },
            //        License = new License { Name = "Licensed under rcos online holdings.", Url = "https://rcosonline.com/" }
            //    });

                //Determine base path for the application.
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                ////Set the comments path for the swagger json and ui.
                //options.IncludeXmlComments(basePath + "\\rcos.reposervice.xml");
            //});

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();


            // Shows UseCors with named policy.
            app.UseCors("CorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            //app.UseSwagger();
            //app.UseSwaggerUi();
        }

        MasterRepository setupMasterRepository(MasterRepository masterRepository) {
            masterRepository.Setup(new RepositoryConfig {
                Connector = ConnectorType.Mongo,
                Description = "wazimerchant",
                DisplayName = "wazimerchant",
                ID = Guid.NewGuid().ToString(),
                Name = "wazimerchant",
                Servers = new List<RepositoryAddress> {
                    new RepositoryAddress { ServerName = "localhost", PortNo = 27017 }
                },
                Type = RepositoryType.master
            });

            //create repository if it does not exist...
            masterRepository.Create();
            return masterRepository;
        }
    }
}
