using CHubBLL.OtherProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchJob
{
    public class JobsManager
    {
        ConfigData config = new ConfigData();
        public JobsManager()
        {

        }

        public bool PackPrint()
        {
            try
            {
                CustPackPrintBLL cpBLL = new CustPackPrintBLL(config.TempPath);
                cpBLL.GetStagedPackList(config.AppUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
