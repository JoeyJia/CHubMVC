using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class TC_PART_HS_AUDIT_BLL
    {
        private readonly TC_PART_HS_AUDIT_DAL dal;

        public TC_PART_HS_AUDIT_BLL()
        {
            dal = new TC_PART_HS_AUDIT_DAL();
        }
        public TC_PART_HS_AUDIT_BLL(CHubEntities db)
        {
            dal = new TC_PART_HS_AUDIT_DAL(db);
        }

        public List<TC_PART_HS_AUDIT> GetAuditLog(string partNo)
        {
            return dal.GetAuditLog(partNo);
        }


    }
}
