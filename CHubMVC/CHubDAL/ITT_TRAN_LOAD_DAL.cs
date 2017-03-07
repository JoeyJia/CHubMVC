using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class ITT_TRAN_LOAD_DAL : BaseDAL
    {
        public ITT_TRAN_LOAD_DAL()
            : base() { }

        public ITT_TRAN_LOAD_DAL(CHubEntities db)
            : base(db) { }


        public List<ITT_TRAN_LOAD> GetTranLoadList(string willBillNo)
        {
            return db.ITT_TRAN_LOAD.Where(a => a.WILL_BILL_NO == willBillNo).OrderBy(a=>a.LOAD_BATCH_TOKEN).ToList();
        }

    }
}
