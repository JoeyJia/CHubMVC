using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class RP_CUST_PACK_TYPE_DAL : BaseDAL
    {
        public RP_CUST_PACK_TYPE_DAL()
            : base() { }

        public RP_CUST_PACK_TYPE_DAL(CHubEntities db)
            : base(db) { }

        public List<ExRPCustPackType> GetCustPackType()
        {
            var result = (
                from c in db.RP_CUST_PACK_TYPE
                select new ExRPCustPackType {
                    CUST_PACK_ID = c.CUST_PACK_ID,
                    PACK_DESC = c.PACK_DESC,
                    ACTIVEIND = c.ACTIVEIND,
                    AUTO_PRINT = c.AUTO_PRINT,
                    CUST_SHORT_NAME = c.CUST_SHORT_NAME
                }
                );

            return result.ToList();
        }

    }
}
