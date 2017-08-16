using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class V_PLABEL_BASE_DAL : BaseDAL
    {
        public V_PLABEL_BASE_DAL()
            : base() { }

        public V_PLABEL_BASE_DAL(CHubEntities db)
            : base(db) { }

        public List<PLabelRow> QueryByPart(string printPartNo, string partNo, string status)
        {
            string sql = @"select
1 COPIES,
QTY_IN_CARTON MOQ ,

 PRINT_PART_NO ,
 PART_NO, 
 DESCRIPTION ,
DESC_CN ,
 SHORT_DESCRIPTION ,
COUNTRY_OF_ORIGIN, 
PART_WEIGHT
from V_PLABEL_BASE where 1=1";//1 SHIP_QTYS,
            if (!string.IsNullOrEmpty(printPartNo))
                sql += string.Format(" and PRINT_PART_NO ='{0}'",printPartNo);

            if (!string.IsNullOrEmpty(partNo))
                sql += string.Format(" and PART_NO ='{0}'", partNo);

            if (!string.IsNullOrEmpty(status))
                sql += string.Format(" and PART_CATALOG='{0}'", status);

            var result = db.Database.SqlQuery<PLabelRow>(sql);
            return result.ToList();
        }


    }
}
