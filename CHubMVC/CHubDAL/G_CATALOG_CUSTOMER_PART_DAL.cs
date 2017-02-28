using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class G_CATALOG_CUSTOMER_PART_DAL : BaseDAL
    {
        public G_CATALOG_CUSTOMER_PART_DAL()
            : base() { }

        public G_CATALOG_CUSTOMER_PART_DAL(CHubEntities db)
            : base(db) { }

        public string GetPartNoFromCustPartNo(string custPartNo,string customerNo)
        {
            CheckCultureInfoForDate();
            string sql =string.Format("select * from G_CATALOG_CUSTOMER_PART where CUSTOMER_PARTNO= '{0}' and CUSTOMER_NO = '{1}'", custPartNo,customerNo);

            List<G_CATALOG_CUSTOMER_PART> modelList = db.Database.SqlQuery<G_CATALOG_CUSTOMER_PART>(sql).ToList();
            if (modelList != null&&modelList.Count>0)
                return modelList[0].CATALOG_NO;
            else
                return null;
        }


    }
}
