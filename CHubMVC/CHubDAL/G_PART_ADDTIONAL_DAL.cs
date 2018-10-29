using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class G_PART_ADDTIONAL_DAL : BaseDAL
    {
        public G_PART_ADDTIONAL_DAL() : base()
        {

        }

        public G_PART_ADDTIONAL_DAL(CHubEntities db) : base(db)
        {

        }

        public List<RP_LABEL_PAPER_TYPE> GetPaperIDList()
        {
            string sql = string.Format(@"select * from RP_LABEL_PAPER_TYPE  where LABEL_CODE = 'IHUB_CN'");
            var result = db.Database.SqlQuery<RP_LABEL_PAPER_TYPE>(sql);
            return result.ToList();
        }


        public List<G_PART_ADDTIONAL> GetPAByPartNo(string Part_NO)
        {
            string sql = string.Format(@"select * from G_PART_ADDTIONAL where 1=1");
            if (!string.IsNullOrEmpty(Part_NO))
                sql += string.Format(@" and PART_NO like '%{0}%'", Part_NO);
            var result = db.Database.SqlQuery<G_PART_ADDTIONAL>(sql);
            return result.ToList();
        }

        public void SavePrtAddt(G_PART_ADDTIONAL gpa)
        {
            base.CheckCultureInfoForDate();
            base.Update(gpa);
        }


        public GPARTDESCArg GetDescByPartNo(string PART_No)
        {
            string sql = string.Format(@"select DESCRIPTION,DESC_CN from G_PART_DESCRIPTION where PART_NO='{0}'", PART_No);
            var result = db.Database.SqlQuery<GPARTDESCArg>(sql).ToList().FirstOrDefault();
            return result;
        }

        public GPARTDESCArg GetDescByPrintPartNo(string PRINT_PART_NO)
        {
            string sql = string.Format(@"select PART_NO,DESCRIPTION,DESC_CN from G_PART_DESCRIPTION where PRINT_PART_NO='{0}'", PRINT_PART_NO);
            var result = db.Database.SqlQuery<GPARTDESCArg>(sql).ToList().FirstOrDefault();
            return result;
        }


    }
}
