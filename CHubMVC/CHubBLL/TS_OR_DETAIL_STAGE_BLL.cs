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
    public class TS_OR_DETAIL_STAGE_BLL
    {
        private readonly TS_OR_DETAIL_STAGE_DAL dal;

        public TS_OR_DETAIL_STAGE_BLL()
        {
            dal = new TS_OR_DETAIL_STAGE_DAL();
        }
        public TS_OR_DETAIL_STAGE_BLL(CHubEntities db)
        {
            dal = new TS_OR_DETAIL_STAGE_DAL(db);
        }

    }
}
