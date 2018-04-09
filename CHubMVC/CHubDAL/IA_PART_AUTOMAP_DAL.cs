using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class IA_PART_AUTOMAP_DAL : BaseDAL
    {
        public IA_PART_AUTOMAP_DAL() : base()
        {

        }

        public IA_PART_AUTOMAP_DAL(CHubEntities db) : base(db)
        {

        }

        public List<IA_PART_AUTOMAP> GetIAMap(string INPUT_PART, string PRTNUM)
        {
            string sql = string.Format(@"select * from IA_PART_AUTOMAP where 1=1");
            if (!string.IsNullOrEmpty(INPUT_PART))
                sql += string.Format(@" and INPUT_PART like '%{0}%'", INPUT_PART);
            if (!string.IsNullOrEmpty(PRTNUM))
                sql += string.Format(@" and PRTNUM like '%{0}%'", PRTNUM);
            var result = db.Database.SqlQuery<IA_PART_AUTOMAP>(sql);
            return result.ToList();
        }

        public bool CheckPRTNUM(string PRTNUM)
        {
            string sql = string.Format(@"select * from G_PART_ADDTIONAL where PRINT_PART_NO ='{0}'", PRTNUM);
            var result = db.Database.SqlQuery<G_PART_ADDTIONAL>(sql).ToList(); ;
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public IA_PART_AUTOMAP GetIaMap(string INPUT_PART)
        {
            this.CheckCultureInfoForDate();
            var result = db.IA_PART_AUTOMAP.Where(i => i.INPUT_PART == INPUT_PART).FirstOrDefault();
            return result;
        }

        public void AddOrUpdateIaMap(IA_PART_AUTOMAP iamap, string Type)
        {
            this.CheckCultureInfoForDate();
            if (Type == "Add")
                base.Add(iamap);
            else
                base.Update(iamap);
        }


        public bool IsExistINPUT_PART(string INPUT_PART)
        {
            this.CheckCultureInfoForDate();
            var result = db.IA_PART_AUTOMAP.Where(i => i.INPUT_PART == INPUT_PART);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }


        public void DeleteIaMap(IA_PART_AUTOMAP iamap)
        {
            this.CheckCultureInfoForDate();
            base.Delete(iamap);
        }


    }
}
