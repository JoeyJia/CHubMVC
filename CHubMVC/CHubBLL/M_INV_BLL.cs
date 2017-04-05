using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class M_INV_BLL
    {
        private readonly M_INV_DAL dal;

        public M_INV_BLL()
        {
            dal = new M_INV_DAL();
        }
        public M_INV_BLL(CHubEntities db)
        {
            dal = new M_INV_DAL(db);
        }

        public List<M_INV> GetInterPDCData(string partNo)
        {
            return dal.GetInterPDCData(partNo);
        }

    }
}
