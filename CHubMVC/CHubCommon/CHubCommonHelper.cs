using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using Oracle.ManagedDataAccess.Types;

namespace CHubCommon
{
    public class CHubCommonHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["OracleDbContext"].ToString();

        public CHubCommonHelper()
        {
            this.CheckCultureInfoForDate();
        }

        /// <summary>
        /// 更新sql
        /// </summary>
        /// <param name="sql"></param>
        public void Update(string sql)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 执行无参的存过
        /// </summary>
        /// <param name="Proc"></param>
        public void ExecProcedureWithoutParams(string Proc)
        {
            //CheckCultureInfoForDate();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(Proc, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }


        /// <summary>
        /// 查询并转list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> Search<T>(string sql) where T : class, new()
        {
            List<T> list = new List<T>();
            DataTable dt = new DataTable();

            //CheckCultureInfoForDate();

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                conn.Close();
            }
            DataTabelToList(list, dt);

            return list;
        }

        /// <summary>
        /// 查询sql转DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlToDataTable(string sql)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// 执行带参函数 sql语句：select 函数 from dual
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecuteFunc(string sql)
        {
            string str = string.Empty;
            //CheckCultureInfoForDate();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                OracleDataReader odr = cmd.ExecuteReader();
                if (odr.HasRows)
                {
                    while (odr.Read())
                    {
                        str = odr[0].ToString();
                    }
                    odr.Close();
                }
                conn.Close();
            }
            return str;
        }

        public string ExecuteSql(string sql)
        {
            string result = string.Empty;
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            return result;
        }



        public static void DataTabelToList<T>(List<T> list, DataTable dt) where T : class, new()
        {
            string tempName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                //定义一个对象
                T t = new T();
                //获取此对象的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach (PropertyInfo pi in propertys)
                {
                    //将属性名赋值给临时变量
                    tempName = pi.Name;
                    //检查DataTable是否包含此属性名（列名==属性名）
                    if (dt.Columns.Contains(tempName))
                    {
                        //取值
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType.FullName == "System.Decimal")
                                value = Convert.ToDecimal(value);
                            if (pi.PropertyType.FullName == "System.Int32")
                                value = Convert.ToInt32(value);
                            if (pi.PropertyType.FullName == "System.Int64")
                                value = Convert.ToInt64(value);
                            if (pi.PropertyType.FullName == "System.String")
                                value = Convert.ToString(value);

                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //将对象添加到集合中
                list.Add(t);
            }
        }

        public static IList<T> DataTableToList<T>(DataTable dt) where T:class,new()
        {
            IList<T> ts = new List<T>();
            Type type = typeof(T);
            string tempName = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }

            return ts;
        }


        /// <summary>
        /// for db.Database.SqlQuery function date CultureInfo check
        /// if CurrentCulture is zh-CN need to change db configuration
        /// otherwise will get wrong date format exception
        /// </summary>
        private void CheckCultureInfoForDate()
        {
            var cur = System.Globalization.CultureInfo.CurrentCulture;//zh-CN
            if (cur.Name == "zh-CN")
            {
                string sql = "alter session set nls_date_language = 'american'";
                Update(sql);
            }
        }


        public void ExecP_MD_SR_NEW(string PART_NO, string PRODUCT_GROUP_ID,string NOTE,string REQ_BY)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_MD_SR_NEW", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_part_no", PART_NO);
                cmd.Parameters.Add("v_PROD_Group", PRODUCT_GROUP_ID);
                cmd.Parameters.Add("v_note", NOTE);
                cmd.Parameters.Add("v_req_by", REQ_BY);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ExecP_Load_Stg_from_RP(string SHIP_TO_LOCATION)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_Load_Stg_from_RP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_abbr", OracleDbType.Int64, 10);
                cmd.Parameters[0].Value = SHIP_TO_LOCATION;
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ExecP_EXP_INV_DISCARD(string COMM_INV_ID)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_EXP_INV_DISCARD", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_inv_id", OracleDbType.Decimal);
                cmd.Parameters[0].Value = decimal.Parse(COMM_INV_ID);
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ExecP_EXP_INV_COMP(string COMM_INV_ID)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_EXP_INV_COMP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_inv_id", OracleDbType.Decimal);
                cmd.Parameters[0].Value = decimal.Parse(COMM_INV_ID);
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ExecP_EXP_STG_LOAD_POST(string LOAD_BATCH)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_EXP_STG_LOAD_POST", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("V_LOAD_batch", OracleDbType.Decimal);
                cmd.Parameters[0].Value = decimal.Parse(LOAD_BATCH);
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void ExecP_EXP_VAT_LOAD_POST(string LOAD_BATCH)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("P_EXP_VAT_LOAD_POST", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("V_LOAD_batch", OracleDbType.Decimal);
                cmd.Parameters[0].Value = decimal.Parse(LOAD_BATCH);
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void RunPRE_WORK_MOBILE_PRINT(string WH_ID, string LODNUM)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("PRE_WORK_MOBILE_PRINT", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("v_wh", OracleDbType.Varchar2);
                cmd.Parameters[0].Value = WH_ID;
                cmd.Parameters[0].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("v_doc", OracleDbType.Varchar2);
                cmd.Parameters[1].Value = LODNUM;
                cmd.Parameters[1].Direction = ParameterDirection.Input;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        

        /// <summary>
        /// 带参数的新增
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public void AddOrUpdateWithParams(string sql, OracleParameter[] param)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 执行带参数的存过
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="param"></param>
        public void ExecProcedure(string ProcName, OracleParameter[] param)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(ProcName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }


        /// <summary>
        /// CheckSecurity检查权限
        /// </summary>
        /// <param name="SECURE_ID"></param>
        /// <param name="APP_USER"></param>
        /// <returns></returns>
        public bool CheckSecurity(string SECURE_ID, string APP_USER)
        {
            string sql = string.Format(@"select * from APP_SECURE_PROC_ASSIGN where SECURE_ID='{0}' and APP_USER='{1}' and ACTIVEIND='Y'", SECURE_ID, APP_USER);
            var result = this.ExecuteSqlToDataTable(sql);
            if (result != null && result.Rows.Count > 0)
                return true;
            else
                return false;
        }

    }
}
