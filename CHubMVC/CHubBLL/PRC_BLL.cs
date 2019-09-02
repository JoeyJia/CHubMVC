using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using System.Data;

namespace CHubBLL
{
    public class PRC_BLL
    {
        private PRC_DAL dal;
        public PRC_BLL()
        {
            dal = new PRC_DAL();
        }

        public DataTable PRCVerify()
        {
            return dal.PRCVerify();
        }
    }
}
