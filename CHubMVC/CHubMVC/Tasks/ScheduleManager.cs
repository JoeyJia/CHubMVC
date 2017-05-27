using Hangfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CHubMVC.Tasks
{
    public class ScheduleManager
    {
        public void AddAllSchedules()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //AM part
            Daily1am d1am = new Daily1am();
            RecurringJob.AddOrUpdate(() => d1am.Execute(), d1am.GetCronString());

            Daily2am d2am = new Daily2am();
            RecurringJob.AddOrUpdate(() => d2am.Execute(), d2am.GetCronString());

            Daily3am d3am = new Daily3am();
            RecurringJob.AddOrUpdate(() => d3am.Execute(), d3am.GetCronString());

            Daily4am d4am = new Daily4am();
            RecurringJob.AddOrUpdate(() => d4am.Execute(), d4am.GetCronString());

            Daily5am d5am = new Daily5am();
            RecurringJob.AddOrUpdate(() => d5am.Execute(), d5am.GetCronString());

            Daily6am d6am = new Daily6am();
            RecurringJob.AddOrUpdate(() => d6am.Execute(), d6am.GetCronString());

            Daily7am d7am = new Daily7am();
            RecurringJob.AddOrUpdate(() => d7am.Execute(), d7am.GetCronString());

            Daily8am d8am = new Daily8am();
            RecurringJob.AddOrUpdate(() => d8am.Execute(), d8am.GetCronString());

            Daily12am d12am = new Daily12am();
            RecurringJob.AddOrUpdate(() => d12am.Execute(), d12am.GetCronString());

            //PM part
            Daily6pm d6pm = new Daily6pm();
            RecurringJob.AddOrUpdate(() => d6pm.Execute(), d6pm.GetCronString());

            Daily7pm d7pm = new Daily7pm();
            RecurringJob.AddOrUpdate(() => d7pm.Execute(), d7pm.GetCronString());

            Daily8pm d8pm = new Daily8pm();
            RecurringJob.AddOrUpdate(() => d8pm.Execute(), d8pm.GetCronString());

            Daily9pm d9pm = new Daily9pm();
            RecurringJob.AddOrUpdate(() => d9pm.Execute(), d9pm.GetCronString());

            Daily10pm d10pm = new Daily10pm();
            RecurringJob.AddOrUpdate(() => d10pm.Execute(), d10pm.GetCronString());

            Daily11pm d11pm = new Daily11pm();
            RecurringJob.AddOrUpdate(() => d11pm.Execute(), d11pm.GetCronString());

            //weekly 2am part
            Monday2am w1_2am = new Monday2am();
            RecurringJob.AddOrUpdate(() => w1_2am.Execute(), w1_2am.GetCronString());

            Tuesday2am w2_2am = new Tuesday2am();
            RecurringJob.AddOrUpdate(() => w2_2am.Execute(), w2_2am.GetCronString());

            Wednesday2am w3_2am = new Wednesday2am();
            RecurringJob.AddOrUpdate(() => w3_2am.Execute(), w3_2am.GetCronString());

            Thursday2am w4_2am = new Thursday2am();
            RecurringJob.AddOrUpdate(() => w4_2am.Execute(), w4_2am.GetCronString());

            Friday2am w5_2am = new Friday2am();
            RecurringJob.AddOrUpdate(() => w5_2am.Execute(), w5_2am.GetCronString());

            Saturday2am w6_2am = new Saturday2am();
            RecurringJob.AddOrUpdate(() => w6_2am.Execute(), w6_2am.GetCronString());

            Sunday2am w0_2am = new Sunday2am();
            RecurringJob.AddOrUpdate(() => w0_2am.Execute(), w0_2am.GetCronString());

            //weekly 10pm part
            Monday10pm w1_10pm = new Monday10pm();
            RecurringJob.AddOrUpdate(() => w1_10pm.Execute(), w1_10pm.GetCronString());

            Tuesday10pm w2_10pm = new Tuesday10pm();
            RecurringJob.AddOrUpdate(() => w2_10pm.Execute(), w2_10pm.GetCronString());

            Wednesday10pm w3_10pm = new Wednesday10pm();
            RecurringJob.AddOrUpdate(() => w3_10pm.Execute(), w3_10pm.GetCronString());

            Thursday10pm w4_10pm = new Thursday10pm();
            RecurringJob.AddOrUpdate(() => w4_10pm.Execute(), w4_10pm.GetCronString());

            Friday10pm w5_10pm = new Friday10pm();
            RecurringJob.AddOrUpdate(() => w5_10pm.Execute(), w5_10pm.GetCronString());

            Saturday10pm w6_10pm = new Saturday10pm();
            RecurringJob.AddOrUpdate(() => w6_10pm.Execute(), w6_10pm.GetCronString());

            Sunday10pm w0_10pm = new Sunday10pm();
            RecurringJob.AddOrUpdate(() => w0_10pm.Execute(), w0_10pm.GetCronString());

            sw.Stop();
            CHubCommon.LogHelper.WriteLog(string.Format("Add 29 tasks, total cost time:{0}",sw.Elapsed.ToString()));
        }
    }
}