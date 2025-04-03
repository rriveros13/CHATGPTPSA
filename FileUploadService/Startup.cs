using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FileUploadService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            string appSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),"appsettings.json");
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(appSettingsFile, false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Program.MyConfig = Configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://www.itti.digital/identity-server";
                    //options.Authority = "https://local.itti.digital:8101/identity-server";
                    options.ApiName = "SersaFileUploadAPI";
                    options.RequireHttpsMetadata = false;
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log-{Date}.txt"))
                .CreateLogger();

            var myContextPath = Configuration["ContextPath"];

            app.UseHttpsRedirection();
            app.UsePathBase(myContextPath);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
