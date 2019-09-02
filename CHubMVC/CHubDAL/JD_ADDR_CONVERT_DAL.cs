using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using CHubCommon;

namespace CHubDAL
{
    public class JD_ADDR_CONVERT_DAL : BaseDAL
    {
        private CHubCommonHelper cchhelper;

        public JD_ADDR_CONVERT_DAL() : base()
        {
            cchhelper = new CHubCommonHelper();
        }

        public JD_ADDR_CONVERT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<JD_ADDR_CONVERT> SearchAdrMap(string GOMS_ADDR, string CREATE_DATE)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from JD_ADDR_CONVERT where 1=1");
            if (!string.IsNullOrEmpty(GOMS_ADDR))
                sql += string.Format(@" and GOMS_ADDR like '%{0}%'", GOMS_ADDR);
            if (!string.IsNullOrEmpty(CREATE_DATE))
                sql += string.Format(@" and CREATE_DATE between to_date('{0}','yyyy-mm-dd hh24:mi:ss') and to_date('{1}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.AddDays(-Convert.ToInt32(CREATE_DATE)).ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            sql += " order by JID desc";

            var result = db.Database.SqlQuery<JD_ADDR_CONVERT>(sql).ToList();

            return result;
        }


        public JD_ADDR_CONVERT SearchAdrMap(string JID)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Format(@"select * from JD_ADDR_CONVERT where JID={0}", Convert.ToDecimal(JID));
            var result = db.Database.SqlQuery<JD_ADDR_CONVERT>(sql).ToList();
            return result.First();
        }

        public void UpdateAdrMap(JD_ADDR_CONVERT adrmap)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Format(@"Update JD_ADDR_CONVERT set CONVERTED_ADDR='{0}',TERRITORY='{1}',UPDATED_BY='{2}',RECORD_DATE=to_date('{3}','yyyy-mm-dd hh24:mi:ss') where JID={4}", 
                                        adrmap.CONVERTED_ADDR,adrmap.TERRITORY, adrmap.UPDATED_BY, adrmap.RECORD_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss"), adrmap.JID);
            cchhelper.ExecuteNonQuery(sql);
        }

        public List<JD_4_CLASS_MST> GetArea(string type, string province = null, string city = null, string county = null)
        {
            //string sql = GetSql(type, province, city, county);
            string sql = string.Format(@"select * from JD_4_CLASS_MST");
            var result = cchhelper.ExecuteSqlToList<JD_4_CLASS_MST>(sql);
            return result;
        }

        public string GetSql(string type, string province = null, string city = null, string county = null)
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case "1":
                    sb.Append(@"select distinct PROVINCE from JD_4_CLASS_MST");
                    break;
                case "2":
                    sb.Append(@"select distinct CITY from JD_4_CLASS_MST where PROVINCE='" + province + "'");
                    break;
                case "3":
                    sb.Append(@"select distinct COUNTY from JD_4_CLASS_MST where PROVINCE='" + province + "' and city='" + city + "'");
                    break;
                case "4":
                    sb.Append(@"select distinct TOWN from JD_4_CLASS_MST where PROVINCE='" + province + "' and city='" + city + "' and COUNTY='" + county + "' ");
                    break;
            }
            return sb.ToString();
        }

        public bool CheckSecurityOfAMSave(string SECURE_ID,string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", SECURE_ID, APP_USER);
            var result = cchhelper.ExecuteSqlToList<APP_SECURE_PROC_ASSIGN>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

    }
}
