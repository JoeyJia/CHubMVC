using System;
using System.Collections.Generic;
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
        /// 
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

    }
}
