using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;

namespace CHubCommon
{
    public class CHubCommonHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["OracleDbContext"].ToString();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql"></param>
        public static void Update(string sql)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Clone();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> Search<T>(string sql) where T : class, new()
        {
            List<T> list = new List<T>();
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
            DataTabelToList(list, dt);

            return list;
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
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //将对象添加到集合中
                list.Add(t);
            }
        }





    }
}
