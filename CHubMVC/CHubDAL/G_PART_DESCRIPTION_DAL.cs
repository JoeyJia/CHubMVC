using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using static CHubCommon.CHubEnum;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class G_PART_DESCRIPTION_DAL : BaseDAL
    {
        public G_PART_DESCRIPTION_DAL()
            : base()
        { }

        public G_PART_DESCRIPTION_DAL(CHubEntities db)
            : base(db)
        { }

        public bool IsPartNoExist(string partNo)
        {
            return db.G_PART_DESCRIPTION.Any(a => a.PART_NO == partNo);
        }

        public G_PART_DESCRIPTION GetPartDescription(string partNo)
        {
            return db.G_PART_DESCRIPTION.FirstOrDefault(a => a.PART_NO == partNo);
        }

        public List<G_PART_DESCRIPTION> fuzzyqueryByPartNo(string fuzzyPartNo)
        {
            return db.G_PART_DESCRIPTION.Where(a => a.PART_NO.Contains(fuzzyPartNo)).ToList();
        }

        public List<G_PART_DESCRIPTION> fuzzyqueryByPartNo_New(string PartNo, string Print_PartNo)
        {
            string sql = string.Format(@"select * from G_PART_DESCRIPTION where 1=1");
            if (!string.IsNullOrEmpty(PartNo))
                sql += string.Format(@" and PART_NO like '%{0}%'", PartNo);
            if (!string.IsNullOrEmpty(Print_PartNo))
                sql += string.Format(@" and PRINT_PART_NO like '%{0}%'", Print_PartNo);
            var result = db.Database.SqlQuery<G_PART_DESCRIPTION>(sql);
            return result.ToList();
        }


        public bool IsInActive(string partNo)
        {
            string status = PartStatusEnum.I.ToString();
            return db.G_PART_DESCRIPTION.Any(a => a.PART_NO == partNo && a.PART_STATUS == status);
        }

        public GPARTDESCArg GetGPartDesc(string PART_NO, string PRINT_PART_NO)
        {
            string sql = string.Format(@"select PART_NO,DESCRIPTION,DESC_CN from G_PART_DESCRIPTION where 1=1");
            if (!string.IsNullOrEmpty(PART_NO))
                sql += string.Format(@" and PART_NO='{0}'", PART_NO);
            if (!string.IsNullOrEmpty(PRINT_PART_NO))
                sql += string.Format(@" and PRINT_PART_NO='{0}'", PRINT_PART_NO);
            var result = db.Database.SqlQuery<GPARTDESCArg>(sql).ToList().FirstOrDefault();
            return result;
        }


    }
}
