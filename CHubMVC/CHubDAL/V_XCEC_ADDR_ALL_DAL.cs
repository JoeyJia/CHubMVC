using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubCommon;

namespace CHubDAL
{
    public class V_XCEC_ADDR_ALL_DAL : BaseDAL
    {
        private CHubCommonHelper cchelper;
        public V_XCEC_ADDR_ALL_DAL() : base()
        {
            cchelper = new CHubCommonHelper();
        }
        public V_XCEC_ADDR_ALL_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_XCEC_ADDR_ALL> SearchXcecAddrAll(string WAREHOUSE, string ADDR_NAME, string ADDR_1, string ADDR_2, string ADDR_3)
        {
            string sql = string.Format(@"select * from V_XCEC_ADDR_ALL where WAREHOUSE='{0}'", WAREHOUSE);
            if (!string.IsNullOrEmpty(ADDR_NAME))
                sql += string.Format(@" and ADDR_NAME like '%{0}%'", ADDR_NAME);
            if (!string.IsNullOrEmpty(ADDR_1))
                sql += string.Format(@" and ADDR_1 like '%{0}%'", ADDR_1);
            if (!string.IsNullOrEmpty(ADDR_2))
                sql += string.Format(@" and ADDR_2 like '%{0}%'", ADDR_2);
            if (!string.IsNullOrEmpty(ADDR_3))
                sql += string.Format(@" and ADDR_3 like '%{0}%'", ADDR_3);

            var result = cchelper.Search<V_XCEC_ADDR_ALL>(sql);

            return result;
        }

        public bool SecureCheck(string User)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='XCEC_MATCH' and APP_USER='{0}' and ACTIVEIND='Y'", User);
            var result = db.Database.SqlQuery<APP_SECURE_PROC_ASSIGN>(sql).ToList();
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public List<V_XCEC_ADDR_ALL> GetXcecAddrAll(string WAREHOUSE, string DEST_LOCATION)
        {
            string sql = string.Format(@"select * from V_XCEC_ADDR_ALL where WAREHOUSE='{0}' and DEST_LOCATION='{1}'", WAREHOUSE, DEST_LOCATION);
            var result = cchelper.Search<V_XCEC_ADDR_ALL>(sql);
            return result;
        }


        public void UpdateXcecAddrAll(V_XCEC_ADDR_ALL addr, string XCEC_ADDR_SEQ, string User)
        {
            string sql = string.Format(@"Update XCEC_INT.XCEC_ADDR_MST Set 
                                        GOMS_CUST_NO='{0}',
                                        BILL_TO_LOCATION='{1}',
                                        SHIP_TO_LOCATION='{2}',
                                        DEST_LOCATION='{3}',
                                        UPDATED_BY='{4}',
                                        MATCH_FLAG='{5}',
                                        ALIAS_NAME='{6}' where XCEC_ADDR_SEQ='{7}'", addr.CUSTOMER_NO, addr.BILL_TO_LOCATION, addr.SHIP_TO_LOCATION, addr.DEST_LOCATION, User, "Y", addr.ALIAS_NAME, XCEC_ADDR_SEQ);
            cchelper.Update(sql);
        }


    }
}
