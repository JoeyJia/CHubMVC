using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubCommon
{
    public class ValueConvert
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
    }
}
