using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class IA_PRT_NOTE_CUST_DAL : BaseDAL
    {
        public IA_PRT_NOTE_CUST_DAL() : base()
        {

        }

        public IA_PRT_NOTE_CUST_DAL(CHubEntities db) : base(db)
        {

        }


        public List<IA_PRT_NOTE_CUST> GetIANOTEC(string PART_NO, string PRINT_PART_NO)
        {
            base.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from IA_PRT_NOTE_CUST where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO ='{0}'", PART_NO);
            if (!string.IsNullOrEmpty(PRINT_PART_NO))
                sql += string.Format(@" and PRINT_PART_NO ='{0}'", PRINT_PART_NO);
            sql += string.Format(@" order by RECORD_DATE desc");

            var result = db.Database.SqlQuery<IA_PRT_NOTE_CUST>(sql).ToList();
            return result;
        }

        public IA_PRT_NOTE_CUST GetIANoteC(string PART_NO, string ADRNAM)
        {
            base.CheckCultureInfoForDate();
            return db.IA_PRT_NOTE_CUST.FirstOrDefault(a => a.PART_NO == PART_NO && a.ADRNAM == ADRNAM);
        }

        public void SaveIANOTEC(IA_PRT_NOTE_CUST ianotec)
        {
            base.CheckCultureInfoForDate();
            base.Update(ianotec);
        }


    }
}
