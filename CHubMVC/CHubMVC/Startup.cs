using CHubBLL;
using CHubCommon;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CHubMVC.Startup))]
namespace CHubMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Hangfire configuration
            var options = new SQLiteStorageOptions();
            string path = System.Configuration.ConfigurationManager.AppSettings["SqlLitePath"];
            string fullPath = System.Web.HttpContext.Current.Server.MapPath(path);
            string conStr = string.Format(CHubConstValues.SqliteConnTemplate, fullPath);

            GlobalConfiguration.Configuration.UseSQLiteStorage(conStr, options);//SQLiteHangfire
            var option = new BackgroundJobServerOptions { WorkerCount = 1 };
            app.UseHangfireServer(option);
            app.UseHangfireDashboard("/hangfire",new DashboardOptions {
                Authorization=new[] { new HangFireAuthorizationFilter() }
            });

            // Add scheduled jobs
            BatchJobs jobs = new BatchJobs();
            string attachFolder = System.Web.HttpContext.Current.Server.MapPath(CHubConstValues.WebEmailAttachFolder);

            RecurringJob.AddOrUpdate(() => jobs.SendM1Mail("M1", attachFolder, null), Cron.Daily(20));


        }
    }
}
