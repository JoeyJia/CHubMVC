using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.WebArg;

namespace CHubDAL
{
  public  class G_KITS_DAL:BaseDAL
    {
        public G_KITS_DAL() : base()
        {

        }

        public G_KITS_DAL(CHubEntities db) : base(db)
        {

        }

        public List<G_KITS> GetGKITSBySearch(string PARENT_PART, string COMPONENT_PART, long LINE_ITEM_NO)
        {
            string sql = string.Format(@"select * from G_KITS where PARENT_PART='{0}' and COMPONENT_PART='{1}' and LINE_ITEM_NO='{2}'", PARENT_PART, COMPONENT_PART, LINE_ITEM_NO);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<G_KITS>(sql);
            return result.ToList();
        }

        public void AddKITS(G_KITS gkits)
        {
            this.CheckCultureInfoForDate();
            base.Add(gkits, true);
        }

        public void UpdateKITS(G_KITS old, G_KITS gkits)
        {
            this.CheckCultureInfoForDate();
            base.Delete(old, true);
            base.Add(gkits, true);
        }

    }
}
