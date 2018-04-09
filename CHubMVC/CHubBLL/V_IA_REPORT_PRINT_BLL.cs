using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_IA_REPORT_PRINT_BLL
    {
        private readonly V_IA_REPORT_PRINT_DAL dal;
        public V_IA_REPORT_PRINT_BLL()
        {
            dal = new V_IA_REPORT_PRINT_DAL();
        }
        public V_IA_REPORT_PRINT_BLL(CHubEntities db)
        {
            dal = new V_IA_REPORT_PRINT_DAL(db);
        }

        public List<V_IA_REPORT_PRINT> IAReportPrint(string LODNUM)
        {
            return dal.IAReportPrint(LODNUM);
        }


    }
}
