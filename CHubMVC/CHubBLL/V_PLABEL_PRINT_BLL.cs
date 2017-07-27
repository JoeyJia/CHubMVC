using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_PLABEL_PRINT_BLL
    {
        private readonly V_PLABEL_PRINT_DAL dal;

        public V_PLABEL_PRINT_BLL()
        {
            dal = new V_PLABEL_PRINT_DAL();
        }
        public V_PLABEL_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_PRINT_DAL(db);
        }

        public List<V_PLABEL_PRINT> GetLabelPrintData(string partNo, string labelCode)
        {
            return dal.GetLabelPrintData(partNo, labelCode);
        }

    }
}
