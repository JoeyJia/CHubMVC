using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CHubCommon;

namespace BatchJob
{
    public  class ConfigData
    {

        public string TempPath
        {
            get
            {
                if (!Directory.Exists("./temp/"))
                {
                    Directory.CreateDirectory("./temp/");
                }
                return Path.GetFullPath("./temp/");
            }
        }


        public string AppUser = "BatchJob";
        

    }
}
