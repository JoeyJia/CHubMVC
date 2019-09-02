using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class MP_ADDRMAP_DAL
    {
        private CHubCommonHelper ccHelper;
        public MP_ADDRMAP_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_E_ADDR_MST> GetV_E_ADDR_MST(V_E_ADDR_MST condition)
        {
            string sql = string.Format(@"select * from V_E_ADDR_MST where 1=1");
            if (!string.IsNullOrEmpty(condition.SHIP_NAME))
                sql += string.Format(@" and SHIP_NAME like '%{0}%'", condition.SHIP_NAME);
            if (!string.IsNullOrEmpty(condition.SHIP_TERRITORY))
                sql += string.Format(@" and SHIP_TERRITORY like '%{0}%'", condition.SHIP_TERRITORY);
            if (!string.IsNullOrEmpty(condition.SHIP_ADDR))
                sql += string.Format(@" and SHIP_ADDR like '%{0}%'", condition.SHIP_ADDR);
            if (!string.IsNullOrEmpty(condition.MAP_STATUS))
                sql += string.Format(@" and MAP_STATUS='{0}'", condition.MAP_STATUS);
            if (!string.IsNullOrEmpty(condition.LastDays))
                sql += string.Format(@" and CREATE_DATE>=sysdate-{0}", Convert.ToInt32(condition.LastDays));
            var result = ccHelper.ExecuteSqlToList<V_E_ADDR_MST>(sql);
            return result;
        }

        public List<V_E_ADDR_MST> GetCustomerAddress(string ADDR_TOKEN, string TO_SYSTEM, string ABBR, decimal DEST_LOCATION)
        {
            string sql = string.Format(@"select * from V_E_ADDR_MST where ADDR_TOKEN='{0}' and TO_SYSTEM='{1}' and ABBR='{2}'", ADDR_TOKEN, TO_SYSTEM, ABBR);
            if (DEST_LOCATION == 0)
                sql += string.Format(@" and DEST_LOCATION is null");
            else
                sql += string.Format(@" and DEST_LOCATION={0}", DEST_LOCATION);
            var result = ccHelper.ExecuteSqlToList<V_E_ADDR_MST>(sql);
            return result;
        }
        public List<V_E_ADDR_MST> GetCustomerAddress(string ADDR_TOKEN)
        {
            string sql = string.Format(@"select * from V_E_ADDR_MST where ADDR_TOKEN='{0}'", ADDR_TOKEN);
            var result = ccHelper.ExecuteSqlToList<V_E_ADDR_MST>(sql);
            return result;
        }

        public List<G_ADDR_SPL> GetGomsAddress(string TO_SYSTEM, string ABBR, string DEST_LOCATION)
        {
            string sql = string.Format(@"select * from G_ADDR_SPL where SYSID='{0}' and ABBREVIATION='{1}' and DEST_LOCATION='{2}'", TO_SYSTEM, ABBR, DEST_LOCATION);
            var result = ccHelper.ExecuteSqlToList<G_ADDR_SPL>(sql);
            return result;
        }

        public List<G_ADDR_SPL> GetGomsAddressByKeyWord(string keyWord, string TO_SYSTEM, string ABBR)
        {
            string sql = string.Format(@"select * from G_ADDR_SPL where (LOCAL_DEST_NAME like '%{0}%' or LOCAL_DEST_ADDR_1 like '%{0}%' or LOCAL_DEST_ADDR_2 like '%{0}%' or LOCAL_DEST_ADDR_3 like '%{0}%' or DEST_CONTACT like '%{0}%' or DEST_PHONE like '%{0}%') and SYSID='{1}' and ABBREVIATION='{2}'", keyWord, TO_SYSTEM, ABBR);
            var result = ccHelper.ExecuteSqlToList<G_ADDR_SPL>(sql);
            return result;
        }

        public void UpdateE_ADDR_MST(string ADDR_TOKEN, string DEST_LOCATION, string APP_USER)
        {
            string sql = string.Format(@"update E_ADDR_MST set DEST_LOCATION='{0}',UPDATED_BY='{1}',RECORD_DATE=sysdate where ADDR_TOKEN='{2}'", DEST_LOCATION, APP_USER, ADDR_TOKEN);
            ccHelper.ExecuteNonQuery(sql);
        }
        public List<G_ADDR_SPL> RefreshGomsAddress(string ADDR_TOKEN, string SYSID, string ABBREVIATION, string DEST_LOCATION, string APP_USER)
        {
            string sql1 = string.Format(@"select * from V_E_ADDR_MST where ADDR_TOKEN='{0}'", ADDR_TOKEN);
            var addrMst = ccHelper.ExecuteSqlToList<V_E_ADDR_MST>(sql1).ToList().First();
            string sql2 = string.Format(@"select * from G_ADDR_SPL where SYSID='{0}' and ABBREVIATION='{1}' and DEST_LOCATION='{2}'", addrMst.TO_SYSTEM, addrMst.ABBR, addrMst.DEST_LOCATION);
            var gomsAddr = ccHelper.ExecuteSqlToList<G_ADDR_SPL>(sql2).ToList();
            return gomsAddr;
        }
    }
}
