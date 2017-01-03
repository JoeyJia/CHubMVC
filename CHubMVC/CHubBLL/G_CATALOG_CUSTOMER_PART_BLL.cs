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
    public class G_CATALOG_CUSTOMER_PART_BLL
    {
        private readonly G_CATALOG_CUSTOMER_PART_DAL dal;

        public G_CATALOG_CUSTOMER_PART_BLL()
        {
            dal = new G_CATALOG_CUSTOMER_PART_DAL();
        }
        public G_CATALOG_CUSTOMER_PART_BLL(CHubEntities db)
        {
            dal = new G_CATALOG_CUSTOMER_PART_DAL(db);
        }

        public string GetPartNoFromCustPartNo(string custPartNo)
        {
            return dal.GetPartNoFromCustPartNo(custPartNo);
        }


    }
}
