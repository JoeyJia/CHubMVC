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
        public static string FirstName = "FIRST_NAME";
        public static string EmailFormat = "{0}@cummins.com";
        public static string DefaultOrderType = OrderTypeEnum.STOCK.ToString();

        public static string IndY = "Y";
        public static string IndN = "N";

        public static string SuccessMsg = "Success";
        public static string FailMsg = "Fail";

        //Stock Tip color
        public static string SatisfyStockColor = "green";
        public static string PartialStockColor = "orange";
        public static string NoStockColor = "red";

        public static string ErrorColor = "red";
        public static string WarningColor = "yellow";
        public static string GoodColor = "green";

        public static string ChubTempFolder = "~/temp/";
        public static string ChubTemplateFolder = "~/Template/";
        public static string ChubIcoFolder = "~/Images/ico/";


        public static string EmailAttachFolder = "./EmailAttach/";
        public static string WebEmailAttachFolder = "/EmailAttach/";
        public static string SqliteConnTemplate = "Data Source={0};Version=3;";

        public static string MailFromAddr = "ihub_automsg@cummins.com";

        public static string HSPartExcelTemplateName = "TC_HS_DB_template_V1.xlsx";
        public static string MPartExcelTemplateName = "TC_MPart_template_V1.xlsx";
        public static string TranLoadExcelTemplateName = "ITT_Tran_Load_template_v1.xlsx";
        public static string CustLoadExcelTemplateName = "ITT_cust_load_template_v1.xlsx";
        public static string EXPExcelTemplateName = "WMS_STAGE_Loading_template_v1.xls";
        public static string EXPFinLoadOneTemplateName = "EXP_VAT_LOAD.xls";
        public static string EXPFinLoadTwoTemplateName = "EXP_VAT_XREF_LOAD.xls";
        public static string EXPFinLoadThreeTemplateName = "EXP_COLLECTION_LOAD.xls";

    }
}
