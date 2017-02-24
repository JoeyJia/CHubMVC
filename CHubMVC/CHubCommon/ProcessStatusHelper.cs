using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public static class ProcessStatusHelper
    {
        static ProcessStatusHelper()
        {
            Reset();
        }

        public static bool IsUsed { get; set; }

        public static int TotalAmout { get; set; }

        public static int CurrentPoint { get; set; }

        public static int  SuccessAmount { get; set; }

        public static int FailAmount { get; set; }

        public static string  Tag { get; set; }


        public static void Reset()
        {
            IsUsed = false;
            TotalAmout = 0;
            CurrentPoint = 0;
            SuccessAmount = 0;
            FailAmount = 0;
            Tag = string.Empty;
        }

    }
}
