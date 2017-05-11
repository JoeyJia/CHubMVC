using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// EF SQL 语句返回 dataTable
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable SqlQueryToDataTatable(this Database db,
                 string sql)
        {
            OracleConnection conn = new OracleConnection();
            //SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            //conn.ConnectionString = db.Connection.ConnectionString;
            //if (conn.State != ConnectionState.Open)
            //{
            //    conn.Open();
            //}

            conn = (OracleConnection)db.Connection;
            //OracleCommand
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;


            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            //conn.Close();
            //conn.Dispose();
            return table;
        }
    }
}
