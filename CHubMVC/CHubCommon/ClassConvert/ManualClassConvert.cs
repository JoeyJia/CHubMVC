using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubModel.ExtensionModel;
using CHubModel;
using CHubDBEntity;
using static CHubCommon.CHubEnum;

namespace CHubCommon
{
    public class ManualClassConvert
    {

        public static TS_OR_HEADER_STAGE ConvertExAliaAddr2HeaderStage(ExVAliasAddr addr)
        {
            TS_OR_HEADER_STAGE hStage = new TS_OR_HEADER_STAGE();
            hStage.ORDER_REQ_NO = 0;//sequence
            hStage.SHIPFROM_SEQ = 0;// 0 or 1
            hStage.TO_SYSTEM = addr.SysID;
            hStage.ALIAS_NAME = addr.AliasName;
            hStage.CUSTOMER_NO = addr.CustomerNo;
            hStage.BILL_TO_LOCATION = (decimal)addr.Bill2Location;
            hStage.SHIP_TO_LOCATION = (decimal)addr.Ship2Location;
            hStage.DEST_LOCATION = 0.00M;//?
            hStage.DUE_DATE = addr.RecordDateOSD.Value;//get form arg
            hStage.ORDER_TYPE = "";//get form arg
            hStage.CUSTOMER_PO_NO = addr.CustomerNo;
            hStage.SPL_IND = "";//?
            hStage.SHIPCOMP_FLAG = "N";//Get from arg
            hStage.ORDER_STATUS = OrderStatusEnum.D.ToString();//draft ind
            hStage.CREATION_DATE = DateTime.Now;
            hStage.CREATED_BY = "";//appuser get from local arg
            hStage.RECORD_DATE = DateTime.Now;//?
            hStage.UPDATED_BY = "";
            hStage.ORDER_NOTES = "";//Get from arg

            return hStage;
        }

    }
}
