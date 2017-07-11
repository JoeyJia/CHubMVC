using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon.Printer
{
    public class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string Name);
    }
}
