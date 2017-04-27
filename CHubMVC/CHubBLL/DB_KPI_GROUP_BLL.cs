using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class DB_KPI_GROUP_BLL
    {
        private readonly DB_KPI_GROUP_DAL dal;

        public DB_KPI_GROUP_BLL()
        {
            dal = new DB_KPI_GROUP_DAL();
        }
        public DB_KPI_GROUP_BLL(CHubEntities db)
        {
            dal = new DB_KPI_GROUP_DAL(db);
        }


        public List<DB_KPI_GROUP> GetKPIGroups()
        {
            return dal.GetKPIGroups();
        }
    }
}
