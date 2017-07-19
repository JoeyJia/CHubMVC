using CHubBLL;
using CHubBLL.OtherProcess;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchJob
{
    class Program
    {
        static void Main(string[] args)
        {

            const string endpoint = "http://localhost:54321";

            using (WebApp.Start<Startup>(endpoint))
            {
                Console.WriteLine();
                Console.WriteLine("{0} Hangfire Server started.", DateTime.Now);
                Console.WriteLine("{0} Dashboard is available at {1}/hangfire", DateTime.Now, endpoint);
                Console.WriteLine();
                Console.WriteLine("{0} Type JOB to add a background job.", DateTime.Now);
                Console.WriteLine("{0} Press exit to exit...", DateTime.Now);


                //string command;
                while (Console.ReadLine() != "exit")
                {
                    Console.WriteLine("exit to end process");
                    //if ("job".Equals(command, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    RecurringJob.AddOrUpdate(() => Console.WriteLine("{0} Background job completed successfully!", DateTime.Now.ToString()), Cron.Minutely);

                    //    //BackgroundJob.Schedule(() => Console.WriteLine("{0} Background job completed successfully!", DateTime.Now.ToString()));
                    //}
                }
            }

            #region last function
            //Console.WriteLine("press any key to start");
            //Console.WriteLine("exit to end process");
            ////if (Console.ReadLine() != "exit")
            ////{
            ////    BatchJobs jobs = new BatchJobs();
            ////    jobs.SendM1Mail();
            ////    Console.WriteLine("Sent M1 Mail!!");
            ////}
            //while (Console.ReadLine() != "exit")
            //{
            //    BatchJobs jobs = new BatchJobs();
            //    jobs.SendM1Mail();
            //    Console.WriteLine("Sent M1 Mail!!");
            //    Console.WriteLine("press any key to start");
            //    Console.WriteLine("exit to end process");
            //}
            #endregion
        }
    }
}
