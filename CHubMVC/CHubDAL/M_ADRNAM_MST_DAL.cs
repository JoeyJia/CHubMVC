using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;


namespace CHubDAL
{
    public class M_ADRNAM_MST_DAL : BaseDAL
    {
        public M_ADRNAM_MST_DAL() : base()
        {

        }

        public M_ADRNAM_MST_DAL(CHubEntities db) : base(db)
        {

        }

        public List<RP_LABEL_TYPE2> GetLabelCode()
        {
            string sql = string.Format(@"select * from RP_LABEL_TYPE2 where ACTIVEIND = 'Y'");
            var result = db.Database.SqlQuery<RP_LABEL_TYPE2>(sql);
            return result.ToList();
        }

        public List<M_ADRNAM_MST> GetCustAddt(string ADRNAM)
        {
            string sql = string.Format(@"select * from M_ADRNAM_MST where 1=1");
            if (!string.IsNullOrEmpty(ADRNAM))
                sql += string.Format(@" and ADRNAM like '%{0}%'", ADRNAM);
            var result = db.Database.SqlQuery<M_ADRNAM_MST>(sql).ToList();
            return result;
        }

        public void SaveCustAddt(M_ADRNAM_MST mam)
        {
            base.CheckCultureInfoForDate();
            base.Update(mam);
        }


    }
}
