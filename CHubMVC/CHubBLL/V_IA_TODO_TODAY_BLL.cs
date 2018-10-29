using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDAL;

namespace CHubBLL
{
    public class V_IA_TODO_TODAY_BLL
    {
        private readonly V_IA_TODO_TODAY_DAL dal;

        public V_IA_TODO_TODAY_BLL()
        {
            dal = new V_IA_TODO_TODAY_DAL();
        }

        public V_IA_TODO_TODAY_BLL(CHubEntities db)
        {
            dal = new V_IA_TODO_TODAY_DAL(db);
        }

        public void RunRefreshProc()
        {
            dal.RunRefreshProc();
        }

        public List<V_IA_TODO_TODAY> GetIATodoToday(string WH_ID, string Cust)
        {
            return dal.GetIATodoToday(WH_ID, Cust);
        }

    }
}
