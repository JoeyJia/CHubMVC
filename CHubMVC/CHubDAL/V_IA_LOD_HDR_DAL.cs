using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_IA_LOD_HDR_DAL : BaseDAL
    {
        public V_IA_LOD_HDR_DAL() : base()
        {

        }

        public V_IA_LOD_HDR_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_IA_LOD_HDR> GetInfoFromHDR(string LODNUM_DISPLAY)
        {
            string sql = string.Format(@"select * from V_IA_LOD_HDR where LODNUM_DISPLAY='{0}'", LODNUM_DISPLAY);
            var result = db.Database.SqlQuery<V_IA_LOD_HDR>(sql);
            return result.ToList();
        }


        public List<V_IA_LOD_HDR> GetVIALODHDR(string WH_ID, string ADRNAM, string LODNUM_DISPLAY, string NEED_SIGN_YN, string IA_STATUS, int CREATE_DATE)
        {
            string StartTime = DateTime.Now.AddDays(-CREATE_DATE).ToString("yyyy-MM-dd 00:00:00");
            string EndTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

            string sql = string.Format(@"select * from V_IA_LOD_HDR  where CREATE_DATE >=to_date('{0}','yyyy-mm-dd hh24:mi:ss') 
                                        and CREATE_DATE <= to_date('{1}','yyyy-mm-dd hh24:mi:ss')", StartTime, EndTime);
            if (!string.IsNullOrEmpty(WH_ID))
                sql += string.Format(@" and WH_ID='{0}'", WH_ID);
            if (!string.IsNullOrEmpty(ADRNAM))
                sql += string.Format(" and ADRNAM like '%{0}%'", ADRNAM);
            if (!string.IsNullOrEmpty(LODNUM_DISPLAY))
                sql += string.Format(" and LODNUM_DISPLAY='{0}'", LODNUM_DISPLAY);
            if (!string.IsNullOrEmpty(NEED_SIGN_YN))
                sql += string.Format(@" and NEED_SIGN_YN='{0}'", NEED_SIGN_YN);
            if (!string.IsNullOrEmpty(IA_STATUS))
                sql += string.Format(@" and IA_STATUS='{0}'", IA_STATUS);

            var result = db.Database.SqlQuery<V_IA_LOD_HDR>(sql);
            return result.ToList();
        }


        public IA_LOD_HDR GetIALODHDR(string LODNUM)
        {
            string sql = string.Format(@"select * from IA_LOD_HDR where LODNUM='{0}'", LODNUM);
            var result = db.Database.SqlQuery<IA_LOD_HDR>(sql).ToList();
            return result.First();
        }

        public void UpdateIALODHDR(IA_LOD_HDR iah)
        {
            this.CheckCultureInfoForDate();
            base.Update(iah);
        }

        public List<IA_STATUS_CODE> GetIAStatus()
        {
            this.CheckCultureInfoForDate();
            return db.IA_STATUS_CODE.ToList();
        }

        public bool CheckUser(string UserName)
        {
            this.CheckCultureInfoForDate();
            //string sql = string.Format(@"select * from APP_USERS where APP_USER ='{0}' and QA_OB_SIGNER='Y'", UserName);
            //var result = db.Database.SqlQuery<APP_USERS>(sql).ToList();
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='IA_SIGN' and APP_USER='{0}' and ACTIVEIND='Y'", UserName);
            var result = db.Database.SqlQuery<APP_SECURE_PROC_ASSIGN>(sql).ToList();
            if (result != null && result.Any())
                return true;
            else
                return false;

        }

    }
}
