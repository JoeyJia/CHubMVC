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
    public class TS_OR_HEADER_BLL
    {
        private readonly TS_OR_HEADER_DAL dal;

        public TS_OR_HEADER_BLL()
        {
            dal = new TS_OR_HEADER_DAL();
        }
        public TS_OR_HEADER_BLL(CHubEntities db)
        {
            dal = new TS_OR_HEADER_DAL(db);
        }

        public bool AddHeader(TS_OR_HEADER toHeader)
        {
            dal.Add(toHeader);
            return true;
        }

    }
}
