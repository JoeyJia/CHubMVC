using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;
using CHubModel.WebArg;

namespace CHubDAL
{
  public  class V_PLABEL_BY_KITS_PRINT_DAL:BaseDAL
    {
        public V_PLABEL_BY_KITS_PRINT_DAL() : base()
        {

        }

        public V_PLABEL_BY_KITS_PRINT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_PLABEL_BY_KITS_PRINT> SearchByKITS(string Label_Code, string Part_No)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_KITS_PRINT where 1=1");
            if (!string.IsNullOrEmpty(Label_Code))
                sql += string.Format(" and LABEL_CODE = '{0}'", Label_Code);
            if (!string.IsNullOrEmpty(Part_No))
                sql += string.Format(" and PARENT_PART = '{0}'", Part_No);
            sql += string.Format(" order by LINE_ITEM_NO");
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_KITS_PRINT>(sql);
            return result.ToList();
        }

        public string GetLabelCodeDesc(string Label_Code)
        {
            var result = db.RP_LABEL_TYPE2.FirstOrDefault(r => r.LABEL_CODE == Label_Code).LABEL_DESC;
            return result;
        }

        public string GetPartNoDescription(string Part_No)
        {
            var result = db.G_UNCATALOG_PART.FirstOrDefault(g => g.PART_NO == Part_No).DESCRIPTION;
            return result;
        }

        public List<V_PLABEL_BY_KITS_PRINT> GetPrintDataByKITS(string LABEL_CODE, string PART_NO)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_KITS_PRINT where  LABEL_CODE = '{0}' and PARENT_PART ='{1}' and DEF_CHECKED = '1'",  LABEL_CODE, PART_NO);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_KITS_PRINT>(sql);
            return result.ToList();
        }

        public int IsExistMore(GKITSArg arg)
        {
            int i = db.G_KITS.AsNoTracking().Where(g => g.PARENT_PART == arg.PARENT_PART).Where(k => k.COMPONENT_PART == arg.COMPONENT_PART && k.LINE_ITEM_NO == arg.LINE_ITEM_NO).Count();
            return i;
        }

        public List<V_PLABEL_BY_KITS_PRINT> GetKITSByVID(string VID)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_KITS_PRINT where VID='{0}'", VID);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_KITS_PRINT>(sql);
            return result.ToList();
        }

    }
}
