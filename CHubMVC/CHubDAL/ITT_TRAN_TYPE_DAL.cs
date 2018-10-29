using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class ITT_TRAN_TYPE_DAL : BaseDAL
    {
        public ITT_TRAN_TYPE_DAL()
            : base()
        { }

        public ITT_TRAN_TYPE_DAL(CHubEntities db)
            : base(db)
        { }

        public List<ITT_TRAN_TYPE> GetTranType()
        {
            string sql = string.Format(@"select * from ITT_TRAN_TYPE");
            return db.Database.SqlQuery<ITT_TRAN_TYPE>(sql).ToList();
            //return db.ITT_TRAN_TYPE.ToList();
        }

    }
}
