using CHubBLL;
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
            Console.WriteLine("press any key to start");
            Console.WriteLine("exit to end process");
            //if (Console.ReadLine() != "exit")
            //{
            //    BatchJobs jobs = new BatchJobs();
            //    jobs.SendM1Mail();
            //    Console.WriteLine("Sent M1 Mail!!");
            //}
            while (Console.ReadLine() != "exit")
            {
                BatchJobs jobs = new BatchJobs();
                jobs.SendM1Mail();
                Console.WriteLine("Sent M1 Mail!!");
                Console.WriteLine("press any key to start");
                Console.WriteLine("exit to end process");
            }
        }
    }
}
