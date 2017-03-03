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
    public class ITT_SHIPPING_H_BLL
    {
        private readonly ITT_SHIPPING_H_DAL dal;

        public ITT_SHIPPING_H_BLL()
        {
            dal = new ITT_SHIPPING_H_DAL();
        }
        public ITT_SHIPPING_H_BLL(CHubEntities db)
        {
            dal = new ITT_SHIPPING_H_DAL(db);
        }

    }
}
