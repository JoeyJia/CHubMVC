using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class ClassConvert
    {
        static Dictionary<string, string> SpecialMapping = new Dictionary<string, string>();

        static ClassConvert()
        {
            //class property, template property(excel)
            SpecialMapping.Add("WILL_BILL_NO", "WAY_BILL_NO");
        }
        public static void ConvertAction(object from, object to, Dictionary<string, string> converter)
        {
            try
            {
                foreach (var item in converter)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        PropertyInfo pi = to.GetType().GetProperty(item.Key);
                        pi.SetValue(to, from.GetType().GetProperty(item.Value).GetValue(from, null));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// "from" obj include or equal "to" obj
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void DrawObj(object from, object to)
        {
            try
            {
                PropertyInfo[] piArray = to.GetType().GetProperties();

                foreach (var item in piArray)
                {
                    if (from.GetType().GetProperty(item.Name) != null)
                    {
                        item.SetValue(to, from.GetType().GetProperty(item.Name).GetValue(from));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DrawObj(object from, object to, List<String> skipList)
        {
            try
            {
                PropertyInfo[] piArray = to.GetType().GetProperties();

                foreach (var item in piArray)
                {
                    if (skipList.Contains(item.Name))
                        continue;
                    if (from.GetType().GetProperty(item.Name) != null)
                    {
                        item.SetValue(to, from.GetType().GetProperty(item.Name).GetValue(from));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<T> ConvertDT2List<T>(DataTable dt, bool hasSpecial = true)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> list = new List<T>();

            List<string> colNames = GetDTColumnName(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object obj = Activator.CreateInstance(typeof(T));
                foreach (var item in properties)
                {
                    try
                    {
                        if (colNames.Contains(item.Name) || SpecialMapping.Keys.ToList().Contains(item.Name))
                        {
                            if (item.PropertyType == typeof(Nullable<decimal>))
                            {
                                item.SetValue(obj, decimal.Parse(dt.Rows[i][item.Name].ToString()));
                            }
                            if (item.PropertyType == typeof(DateTime) || item.PropertyType == typeof(Nullable<DateTime>))
                            {
                                //dt form NPOI , the format of datetime is "yy-M月-yyyy" 
                                //tem.SetValue(obj, DateTime.ParseExact(dt.Rows[i][item.Name].ToString().Replace("月", ""),"dd-M-yyyy", System.Globalization.CultureInfo.CurrentCulture));
                                item.SetValue(obj, DateTime.Parse(dt.Rows[i][item.Name].ToString(), System.Globalization.CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                if (hasSpecial && SpecialMapping.Keys.ToList().Contains(item.Name))
                                {
                                    item.SetValue(obj, dt.Rows[i][SpecialMapping[item.Name]]);
                                }
                                else
                                    item.SetValue(obj, dt.Rows[i][item.Name]);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                list.Add((T)obj);
            }
            return list;
        }


        private static List<string> GetDTColumnName(DataTable dt)
        {
            List<string> result = new List<string>();
            if (dt.Columns == null || dt.Columns.Count == 0)
                return null;

            foreach (DataColumn item in dt.Columns)
            {
                result.Add(item.ColumnName);
            }
            return result;
        }


        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            //定义一个集合
            List<T> list = new List<T>();
            //定义一个临时变量
            string tempName = string.Empty;
            //遍历DataTable所有的数据行
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

            return list;
        }



    }
}
