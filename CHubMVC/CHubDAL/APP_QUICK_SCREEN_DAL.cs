using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.WebArg;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using CHubCommon;

namespace CHubDAL
{
    public class APP_QUICK_SCREEN_DAL : BaseDAL
    {
        private CHubCommonHelper cchelper;
        public APP_QUICK_SCREEN_DAL() : base()
        {
            cchelper = new CHubCommonHelper();
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

        public DataTable GetTableColum(string TABLE_NAME)
        {
            string sql = string.Format(@"select * from user_tab_columns where TABLE_NAME='{0}' order by column_id", TABLE_NAME);
            DataTable dt = cchelper.ExecuteSqlToDataTable(sql);

            return dt;

            //var result = db.Database.SqlQuery<TableColumnListArg>(sql).ToList();
            //return result;
        }

        public DataTable GetTableDatas(string TABLE_NAME)
        {
            string sql = string.Format(@"select * from {0}", TABLE_NAME);
            //using (OracleConnection conn = new OracleConnection(db.Database.Connection.ConnectionString))
            //{
            //    OracleCommand cmd = new OracleCommand(sql, conn);
            //    cmd.CommandType = CommandType.Text;
            //    conn.Open();
            //    OracleDataAdapter oda = new OracleDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    oda.Fill(dt);
            //    return dt;
            //}
            DataTable dt = cchelper.ExecuteSqlToDataTable(sql);
            return dt;
        }


        public List<string> GetQUICK_SCREEN()
        {
            string sql = string.Format(@"select QUICK_SCREEN from APP_QUICK_SCREEN where ACTIVEIND='Y'");
            var result = db.Database.SqlQuery<string>(sql).ToList();
            return result;
        }

        public APP_QUICK_SCREEN GetTableName(string QUICK_SCREEN)
        {
            string sql = string.Format(@"select * from APP_QUICK_SCREEN where QUICK_SCREEN ='{0}'", QUICK_SCREEN);
            var result = db.Database.SqlQuery<APP_QUICK_SCREEN>(sql).ToList().First();
            return result;
        }


        public string GetPRIMARY_KEY(string TABLE_NAME)
        {
            string sql = string.Format(@"select col.column_name from user_constraints con,user_cons_columns col
                                        where con.constraint_name=col.constraint_name and con.constraint_type='P'
                                        and col.table_name='{0}'",TABLE_NAME);
            var result = db.Database.SqlQuery<string>(sql).ToList().FirstOrDefault();
            return result;
        }

        public void UpdateOrInsertQuickScreen(string sql)
        {
            cchelper.Update(sql);
        }

    }
}
