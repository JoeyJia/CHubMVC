using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
   public class V_PLABEL_BY_PART_PRINT_DAL:BaseDAL
    {
        public V_PLABEL_BY_PART_PRINT_DAL():base()
        {

        }

        public V_PLABEL_BY_PART_PRINT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_PLABEL_BY_PART_PRINT> GetSearchByParts(string Label_TYPE, string PRTNUM, string Part_No)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_PART_PRINT where 1=1");
            if (!string.IsNullOrEmpty(Label_TYPE))
                sql += string.Format(@" and LABEL_CODE ='{0}'", Label_TYPE);
            if (!string.IsNullOrEmpty(PRTNUM))
                sql += string.Format(@" and PRINT_PART_NO ='{0}'", PRTNUM);
            if (!string.IsNullOrEmpty(Part_No))
                sql += string.Format(@" and PART_NO ='{0}'", Part_No);

            //need
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_PART_PRINT>(sql);
            return result.ToList();
        }


        public List<V_PLABEL_BY_PART_PRINT> GetPrintDataByPart(List<string> partNoList, string LabelTYPE)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_PART_PRINT where PART_NO in ({0}) and LABEL_CODE = '{1}'", partNoList.ToSqlInStr(),LabelTYPE);
            //need
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_PART_PRINT>(sql);
            return result.ToList();
        }

    }
}
