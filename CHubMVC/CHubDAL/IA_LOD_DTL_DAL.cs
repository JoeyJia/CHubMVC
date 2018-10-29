using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class IA_LOD_DTL_DAL : BaseDAL
    {
        public IA_LOD_DTL_DAL() : base()
        {

        }

        public IA_LOD_DTL_DAL(CHubEntities db) : base(db)
        {

        }

        public List<IA_LOD_DTL> GetIaLodDtl(string LODNUM, string PRTNUM)
        {
            string sql = string.Format(@"select * from IA_LOD_DTL where LODNUM='{0}' and PRTNUM='{1}'", LODNUM, PRTNUM);
            var result = db.Database.SqlQuery<IA_LOD_DTL>(sql);
            return result.ToList();
        }

        public string GetNewPRTNUM(string PRTNUM)
        {
            string sql = string.Format(@"select GET_PARTNO('{0}') from dual", PRTNUM);
            var result = db.Database.SqlQuery<string>(sql).ToList();
            return result[0];
        }


        public void SaveIALODDTL(IA_LOD_DTL iad)
        {
            this.CheckCultureInfoForDate();
            base.Update(iad);
        }


    }
}
