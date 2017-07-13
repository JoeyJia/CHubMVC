using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    static public class ValueConvert
    {
        static public string BoolToNY(bool bValue)
        {
            return bValue ? "Y" : "N";
        }

        static public string BoolToNY(bool? bValue)
        {
            return (bValue ?? false) ? "Y" : "N";
        }

        static public bool NYToBool(string sValue)
        {
            return sValue == "Y" ? true : false;
        }

        static public string GetColorFullName(string shortNfame)
        {
            switch (shortNfame)
            {
                case "R":
                    return "red";
                case "Y":
                    return "yellow";
                case "G":
                    return "green";
                default:
                    return "green";
            }
        }


        #region extension  method
        static public string ToSqlInStr(this List<string> destList)
        {
            string result = string.Empty;
            if (destList == null || destList.Count == 0)
                return "''";

            for (int i = 0; i < destList.Count; i++)
            {
                if (i == 0)
                    result += string.Format("'{0}'", destList[i]);
                else
                    result += string.Format(",'{0}'", destList[i]);
            }
            return result;
        }


        #endregion
    }
}
