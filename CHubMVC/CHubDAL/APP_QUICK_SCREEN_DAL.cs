using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.WebArg;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class APP_QUICK_SCREEN_DAL : BaseDAL
    {
        public APP_QUICK_SCREEN_DAL() : base()
        {

        }

        public APP_QUICK_SCREEN_DAL(CHubEntities db) : base(db)
        {

        }

        public List<APP_QUICK_SCREEN> GetAPPQUICKSCREENList()
        {
            string sql = string.Format(@"select * from APP_QUICK_SCREEN where ACTIVEIND='Y'");
            var result = db.Database.SqlQuery<APP_QUICK_SCREEN>(sql).ToList();
            return result;
        }

        public List<TableColumnListArg> GetTableColum(string TABLE_NAME)
        {
            string sql = string.Format(@"select TABLE_NAME,COLUMN_NAME,DATA_TYPE from user_tab_columns where TABLE_NAME='{0}' order by column_id", TABLE_NAME);
            var result = db.Database.SqlQuery<TableColumnListArg>(sql).ToList();
            return result;
        }

        public DataTable GetTableDatas(string TABLE_NAME)
        {
            string sql = string.Format(@"select * from {0}", TABLE_NAME);
            using (OracleConnection conn = new OracleConnection(db.Database.Connection.ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                return dt;
            }
        }


        
    }
}
