using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class V_IA_LOD_BASE_DAL : BaseDAL
    {
        public V_IA_LOD_BASE_DAL() : base()
        {

        }

        public V_IA_LOD_BASE_DAL(CHubEntities db) : base(db)
        {

        }

        public List<CHubDBEntity.UnmanagedModel.V_IA_LOD_BASE> GetInfoFromBASE(string LODNUM_DISPLAY)
        {
            string sql = string.Format(@"select * from V_IA_LOD_BASE where LODNUM_DISPLAY='{0}'", LODNUM_DISPLAY);
            var result = db.Database.SqlQuery<CHubDBEntity.UnmanagedModel.V_IA_LOD_BASE>(sql);
            return result.ToList();
        }

        public void RunProc(string LODNUM, string UserID)
        {
            db.P_IA_NEW(LODNUM, UserID);
        }


    }
}
