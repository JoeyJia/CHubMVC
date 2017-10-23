using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
  public  class V_PLABEL_BY_ASN_PRINT_DAL:BaseDAL
    {
        public V_PLABEL_BY_ASN_PRINT_DAL() : base()
        {

        }

        public V_PLABEL_BY_ASN_PRINT_DAL(CHubEntities db) : base(db)
        {

        }


        public List<V_PLABEL_BY_ASN_PRINT> GetSearchByASN(string LABEL_CODE, string ASN_NO, string PRINT_PART_NO, string PART_NO)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_ASN_PRINT where LABEL_CODE='{0}' and ASN_NO ='{1}'", LABEL_CODE, ASN_NO);
            if (!string.IsNullOrEmpty(PRINT_PART_NO))
                sql += string.Format(@" and PRINT_PART_NO = '{0}'", PRINT_PART_NO);
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO = '{0}'", PART_NO);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_ASN_PRINT>(sql);
            return result.ToList();
        }

        public List<V_PLABEL_BY_ASN_PRINT> GetPrintDatasByASN(List<string> VID, string LabelTYPE, string ASN_NO, string PART_NO_ASN, string PRINT_PART_NO_ASN)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_ASN_PRINT where LABEL_CODE ='{0}' and ASN_NO = '{1}' and VID in ({2})", LabelTYPE, ASN_NO, VID.ToSqlInStr());
            if (!string.IsNullOrEmpty(PART_NO_ASN))
                sql += string.Format(@"  and PART_NO = '{0}'", PART_NO_ASN);
            if (!string.IsNullOrEmpty(PRINT_PART_NO_ASN))
                sql += string.Format(@" and PRINT_PART_NO ='{0}'", PRINT_PART_NO_ASN);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_ASN_PRINT>(sql);
            return result.ToList();
        }


        public string GetCOMPANY_NAMEByASN_NO(string ASN_NO)
        {
            string COMPANY_NAME = db.V_PLABEL_BY_ASN_PRINT.FirstOrDefault(v => v.ASN_NO == ASN_NO).COMPANY_NAME;
            return COMPANY_NAME;
        }

    }
}
