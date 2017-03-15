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
            catch(Exception ex)
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

        public static void DrawObj(object from, object to,List<String> skipList)
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


        public static List<T> ConvertDT2List<T>(DataTable dt) 
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object obj = Activator.CreateInstance(typeof(T));
                foreach (var item in properties)
                {
                    try
                    {
                        if (dt.Rows[i][item.Name] != null)
                        {
                            if (item.PropertyType == typeof(Nullable<decimal>))
                            {
                                item.SetValue(obj, decimal.Parse(dt.Rows[i][item.Name].ToString()));
                            }
                            if (item.PropertyType == typeof(DateTime) || item.PropertyType == typeof(Nullable<DateTime>))
                            {
                                //dt form NPOI , the format of datetime is "yy-M月-yyyy" 
                                //tem.SetValue(obj, DateTime.ParseExact(dt.Rows[i][item.Name].ToString().Replace("月", ""),"dd-M-yyyy", System.Globalization.CultureInfo.CurrentCulture));
                                item.SetValue(obj, DateTime.Parse(dt.Rows[i][item.Name].ToString(),System.Globalization.CultureInfo.InvariantCulture));
                            }
                            else
                                item.SetValue(obj, dt.Rows[i][item.Name]);
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
    }
}
