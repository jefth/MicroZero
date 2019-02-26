using System;
using System.IO;
using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.ZeroNet.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZeroNet.Http.Gateway
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ZeroApplication.CheckOption();
            RouteOption.ConfigFileName = Path.Combine(ZeroApplication.Config.ConfigFolder, "route_config.json");
            ZeroTrace.SystemLog("HttpGateway", RouteOption.ConfigFileName);
            RouteOption.CheckOption();
        }

        public static IConfiguration Configuration { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IocHelper.SetServiceCollection(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseFileServer();

            string ext = ConfigurationManager.AppSettings["WechatPath"];
            LogRecorder.SystemLog(ext);
            RouteApp.Extends.Add(ext, () => new WechatRouter());
            RouteApp.Extends.Add(ext + "/", () => new WechatRouter());
            RouteApp.Initialize();
            app.Run(RouteApp.Call);
        }
    }
}