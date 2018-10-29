using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_IA_PRT_NOTE_CUST_ADDNEW_DAL : BaseDAL
    {
        public V_IA_PRT_NOTE_CUST_ADDNEW_DAL() : base() { }

        public V_IA_PRT_NOTE_CUST_ADDNEW_DAL(CHubEntities db) : base(db) { }

        public List<V_IA_PRT_NOTE_CUST_ADDNEW> GetIANOTECNEW(string PART_NO, string ADRNAM)
        {
            base.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from V_IA_PRT_NOTE_CUST_ADDNEW where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO='{0}'", PART_NO);
            if (!string.IsNullOrEmpty(ADRNAM))
                sql += string.Format(@" and ADRNAM like '%{0}%'", ADRNAM);
            sql += string.Format(@" and rownum<=50");
            var result = db.Database.SqlQuery<V_IA_PRT_NOTE_CUST_ADDNEW>(sql).ToList();
            return result;
        }

        public void IANOTECSaveNew(string PART_NO, string ADRNAM, string QC_NOTE, string userID)
        {
            db.P_IANOTEC_NEW(PART_NO, ADRNAM, QC_NOTE, userID);
        }


    }
}
