using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
   public class G_UNCATALOG_PART_DAL:BaseDAL
    {
        public G_UNCATALOG_PART_DAL() : base()
        {

        }

        public G_UNCATALOG_PART_DAL(CHubEntities db) : base(db)
        {

        }

        public List<G_UNCATALOG_PART> GetUncatalogPart(string PART_NO, string PRINT_PART_NO)
        {
            string sql = string.Format(@"select * from G_UNCATALOG_PART where PART_NO ='{0}' and PRINT_PART_NO ='{1}'", PART_NO, PRINT_PART_NO);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<G_UNCATALOG_PART>(sql);
            return result.ToList();
        }

        public void AddUncatalogPart(G_UNCATALOG_PART gp)
        {
            this.CheckCultureInfoForDate();
            base.Add(gp, true);
        }

        public void ModifyUncatalogPart(G_UNCATALOG_PART gp)
        {
            this.CheckCultureInfoForDate();
            base.Update(gp, true);
        }

        public List<G_UNCATALOG_PART> GetUncatalogByPART_NO(string PART_NO)
        {
            string sql = string.Format(@"select * from G_UNCATALOG_PART where PART_NO ='{0}'", PART_NO);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<G_UNCATALOG_PART>(sql);
            return result.ToList();
        }


    }
}
