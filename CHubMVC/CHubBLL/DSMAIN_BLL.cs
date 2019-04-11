using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;
using CHubModel.WebArg;

namespace CHubBLL
{
    public class DSMAIN_BLL
    {
        private DSMAIN_DAL dal;
        public DSMAIN_BLL()
        {
            dal = new DSMAIN_DAL();
        }
        public List<V_DS_ORDER_BASE> DSMAINSearch(DSMAINArg arg)
        {
            return dal.DSMAINSearch(arg);
        }
        public void RunP_DS_STatus_REFRESH()
        {
            dal.RunP_DS_STatus_REFRESH();
        }
        public List<V_IHUB_OA_BASE> DSMAINMore(string PO_NO, string PART_NO)
        {
            return dal.DSMAINMore(PO_NO, PART_NO);
        }
    }
}
