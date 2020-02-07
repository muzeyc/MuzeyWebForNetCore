using System.IO;
﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using MuzeyAngular.Configuration;
using MuzeyAngular.Identity;

using Abp.AspNetCore.SignalR.Hubs;
using System.Threading;
using MuzeySignalr;
using Microsoft.AspNetCore.SignalR;
using MuzeyThread;
using S7.Net;

namespace MuzeyAngular.Web.Host.Startup
{
    public class Startup
    {
        //本地
        private const string _defaultCorsPolicyName = "localhost";
        //发布
        //private const string _defaultCorsPolicyName = "10.26.184.21";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            );

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "MuzeyAngular API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<MuzeyAngularWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

app.Use(async (context, next) =>                {                    await next();                    if (context.Response.StatusCode == 404                        && !Path.HasExtension(context.Request.Path.Value)                        && !context.Request.Path.Value.StartsWith("/api/services", StringComparison.InvariantCultureIgnoreCase))                    {                        context.Request.Path = "/index.html";                        await next();                    }                });
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();


            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
                routes.MapHub<MuzeySignalrCommon>("/muzeySignalr");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "MuzeyAngular API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("MuzeyAngular.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger

            //MuzeyThreadManager mtm = new MuzeyThreadManager();
            //mtm.ThreadStart(new System.Collections.Generic.List<string>() { "PBSINCAR", "PBSOUTCAR", "PBSOUTCAREEND", "WBSINCAR", "WBSOUTCAR" });
            //Plc plcPbs = new Plc(CpuType.S71500, "10.25.96.12", 0, 1);
            //var pbsController = false;
            //Plc plcWbs = new Plc(CpuType.S71500, "10.25.248.72", 0, 1);
            //var wbsController = false;
            TimerCallback callback = (x) => {

                //if (!plcPbs.IsConnected)
                //{
                //    pbsController = false;
                //    plcPbs.Open();
                //}
                //else
                //{
                //    var cv = plcPbs.Read("DB2017.DBX76.2").ToString();
                //    var cvBool = cv == "True" ? true : false;
                //    if (cvBool != pbsController)
                //    {
                //        pbsController = cvBool;
                //        if (cvBool)
                //        {
                //            mtm.ThreadStart(new System.Collections.Generic.List<string>() { "PBSINROAD", "PBSOUTROAD" });
                //        }
                //        else
                //        {
                //            mtm.ThreadClose(new System.Collections.Generic.List<string>() { "PBSINROAD", "PBSOUTROAD" });
                //        }
                //    }
                //    MsgMQ.MqAdd(new MsgMQModel() { user = "ALL", message = string.Format("RCArea#{0}→RCCurMode#{1}", "PBS", cv) });
                //}

                //if (!plcWbs.IsConnected)
                //{
                //    wbsController = false;
                //    plcWbs.Open();
                //}
                //else
                //{
                //    var cv = plcWbs.Read("DB39000.DBX86.3").ToString();
                //    var cvBool = cv == "True" ? true : false;
                //    if (cvBool != wbsController)
                //    {
                //        wbsController = cvBool;
                //        if (cvBool)
                //        {
                //            mtm.ThreadStart(new System.Collections.Generic.List<string>() { "WBSINROAD", "WBSOUTROAD", "WBSLIFT" });
                //        }
                //        else
                //        {
                //            mtm.ThreadClose(new System.Collections.Generic.List<string>() { "WBSINROAD", "WBSOUTROAD", "WBSLIFT" });
                //        }
                //    }
                //    MsgMQ.MqAdd(new MsgMQModel() { user = "ALL", message = string.Format("RCArea#{0}→RCCurMode#{1}", "WBS", plcWbs.Read("DB39000.DBX86.3").ToString()) });
                //}

                //var hub = app.ApplicationServices.GetService<IHubContext<MuzeySignalrCommon>>();
                //for (int i = 0; i < 50; i++)
                //{
                //    lock (MsgMQ.msgList)
                //    {
                //        if (MsgMQ.msgList.Count > 0)
                //        {
                //            hub.Clients.All.SendAsync("MuzeySignalr", MsgMQ.msgList[0].user, MsgMQ.msgList[0].message);
                //            MsgMQ.MqRemove();
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}
            };

            var timer = new Timer(callback);
            timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));
        }
    }
}
