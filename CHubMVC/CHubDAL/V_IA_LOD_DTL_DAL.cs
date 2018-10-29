using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_IA_LOD_DTL_DAL : BaseDAL
    {
        public V_IA_LOD_DTL_DAL() : base()
        {

        }

        public V_IA_LOD_DTL_DAL(CHubEntities db) : base(db)
        {

        }

        public List<CHubDBEntity.UnmanagedModel.V_IA_LOD_DTL> GetInfoLODDTL(string LODNUM)
        {
            string sql = string.Format(@"select LODNUM,PRTNUM,NOTE,QTY_DISPLAY,IA_QTY,IA_CODE1,IA_CODE2 from V_IA_LOD_DTL where LODNUM='{0}'", LODNUM);
            var result = db.Database.SqlQuery<CHubDBEntity.UnmanagedModel.V_IA_LOD_DTL>(sql);
            return result.ToList();
        }


        public List<V_IA_LOD_DTL> GetVIALODDTL(string LODNUM)
        {
            string sql = string.Format(@"select * from V_IA_LOD_DTL where LODNUM='{0}'", LODNUM);
            var result = db.Database.SqlQuery<V_IA_LOD_DTL>(sql);
            return result.ToList();
        }




    }
}
