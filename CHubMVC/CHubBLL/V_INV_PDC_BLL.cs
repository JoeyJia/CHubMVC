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
    public class V_INV_PDC_BLL
    {
        private readonly V_INV_PDC_DAL dal;

        public V_INV_PDC_BLL()
        {
            dal = new V_INV_PDC_DAL();
        }
        public V_INV_PDC_BLL(CHubEntities db)
        {
            dal = new V_INV_PDC_DAL(db);
        }

        public List<V_INV_PDC> GetPDCData(string partNo)
        {
            return dal.GetPDCData(partNo);
        }

    }
}
