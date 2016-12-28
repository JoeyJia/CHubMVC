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
            foreach (var item in converter)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    PropertyInfo pi = to.GetType().GetProperty(item.Key);
                    pi.SetValue(to, from.GetType().GetProperty(item.Value).GetValue(from, null));
                }
            }
        }

    }
}
