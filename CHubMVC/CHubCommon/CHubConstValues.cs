using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHubCommon.CHubEnum;

namespace CHubCommon
{
    public class CHubConstValues
    {
        public static string SessionUser = "AppUser";
        public static string EmailFormat = "{0}@cummins.com";
        public static string DefaultOrderType = OrderTypeEnum.STOCK.ToString();

        public static string IndY = "Y";
        public static string IndN = "N";

        //Stock Tip color
        public static string SatisfyStockColor = "green";
        public static string PartialStockColor = "orange";
        public static string NoStockColor = "red";

        public static string ErrorColor = "red";
        public static string WarningColor = "yellow";

        public static string ChubTempFolder = "~/temp/";
        public static string ChubTemplateFolder = "~/Template/";

        public static string HSPartExcelTemplateName = "TC_HS_DB_template_V1.xlsx";
        public static string MPartExcelTemplateName = "TC_MPart_template_V1.xlsx";

    }
}
