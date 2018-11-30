using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class Chart_BLL
    {
        private Chart_DAL dal;
        public Chart_BLL()
        {
            dal = new Chart_DAL();
        }
        public List<V_DASH_TMS01> GetTMSData()
        {
            return dal.GetTMSData();
        }
        public List<V_DASH_SHIP11> GetV_DASH_SHIP11()
        {
            return dal.GetV_DASH_SHIP11();
        }
        public List<V_DASH_SHIP12> GetV_DASH_SHIP12()
        {
            return dal.GetV_DASH_SHIP12();
        }
        public List<V_DASH_SHIP21> GetV_DASH_SHIP21()
        {
            return dal.GetV_DASH_SHIP21();
        }
        public List<V_DASH_SHIP22> GetV_DASH_SHIP22()
        {
            return dal.GetV_DASH_SHIP22();
        }
    }
}
