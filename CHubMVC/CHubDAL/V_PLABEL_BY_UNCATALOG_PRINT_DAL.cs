using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
  public  class V_PLABEL_BY_UNCATALOG_PRINT_DAL:BaseDAL
    {
        public V_PLABEL_BY_UNCATALOG_PRINT_DAL() : base()
        {

        }
        public V_PLABEL_BY_UNCATALOG_PRINT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_PLABEL_BY_UNCATALOG_PRINT> GetSearchByUncatalog(string Label_TYPE, string PRINT_PART_NO_UParts, string PART_NO_UParts)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_UNCATALOG_PRINT where LABEL_CODE ='{0}'", Label_TYPE);
            if (!string.IsNullOrEmpty(PRINT_PART_NO_UParts))
                sql += string.Format(@" and PRINT_PART_NO = '{0}'", PRINT_PART_NO_UParts);
            if (!string.IsNullOrEmpty(PART_NO_UParts))
                sql += string.Format(@" and PART_NO = '{0}'",PART_NO_UParts);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_UNCATALOG_PRINT>(sql);
            return result.ToList();
        }

        public List<V_PLABEL_BY_UNCATALOG_PRINT> GetPrintDataByUncatalog(List<string> partNoList, string LabelTYPE, string PRINT_PART_NO_UParts, string PART_NO_UParts)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_UNCATALOG_PRINT where PART_NO in ({0}) and LABEL_CODE = '{1}'", partNoList.ToSqlInStr(), LabelTYPE);
            if (!string.IsNullOrEmpty(PRINT_PART_NO_UParts))
                sql += string.Format(@" and PRINT_PART_NO = '{0}'", PRINT_PART_NO_UParts);
            if (!string.IsNullOrEmpty(PART_NO_UParts))
                sql += string.Format(@" and PART_NO ='{0}'", PART_NO_UParts);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_UNCATALOG_PRINT>(sql);
            return result.ToList();
        }


    }
}
