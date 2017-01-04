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

        public static TS_OR_HEADER_STAGE ConvertExAliaAddr2HeaderStage(ExVAliasAddr addr, string seq, string dueDate,string orderType,string shipCompFlag,string customerPONO, string orderNote,string appUser,bool IsAltAddr = false)
        {
            TS_OR_HEADER_STAGE hStage = new TS_OR_HEADER_STAGE();
            hStage.ORDER_REQ_NO = string.IsNullOrEmpty(seq) ? 0 : decimal.Parse(seq);//sequence
            hStage.SHIPFROM_SEQ = IsAltAddr?1:0;// 0 or 1
            hStage.TO_SYSTEM = addr.SysID;
            hStage.ALIAS_NAME = addr.AliasName;
            hStage.CUSTOMER_NO = addr.CustomerNo;
            hStage.BILL_TO_LOCATION = (decimal)addr.Bill2Location;
            hStage.SHIP_TO_LOCATION = (decimal)addr.Ship2Location;
            hStage.DEST_LOCATION = addr.DestLocation;
            hStage.DUE_DATE = DateTime.Parse(dueDate);
            hStage.ORDER_TYPE = orderType;//get form arg
            hStage.CUSTOMER_PO_NO = customerPONO;//--get form arg?
            hStage.SPL_IND = "Y";//?
            hStage.SHIPCOMP_FLAG = shipCompFlag;//Get from arg
            hStage.ORDER_STATUS = OrderStatusEnum.D.ToString();//draft ind
            hStage.CREATION_DATE = DateTime.Now;
            hStage.CREATED_BY = appUser;//appuser get from local arg
            hStage.RECORD_DATE = DateTime.Now;//?
            hStage.UPDATED_BY = appUser;
            hStage.ORDER_NOTES = orderNote;//Get from arg

            return hStage;
        }

        public static TS_OR_HEADER ConvertExAliaAddr2Header(ExVAliasAddr addr,string seq, string dueDate, string orderType, string shipCompFlag, string customerPONO, string orderNote, string appUser, bool IsAltAddr = false)
        {
            TS_OR_HEADER header = new TS_OR_HEADER();
            header.ORDER_REQ_NO = string.IsNullOrEmpty(seq)?0:decimal.Parse(seq);//sequence
            header.SHIPFROM_SEQ = IsAltAddr ? 1 : 0;// 0 or 1
            header.TO_SYSTEM = addr.SysID;
            header.ALIAS_NAME = addr.AliasName;
            header.CUSTOMER_NO = addr.CustomerNo;
            header.BILL_TO_LOCATION = (decimal)addr.Bill2Location;
            header.SHIP_TO_LOCATION = (decimal)addr.Ship2Location;
            header.DEST_LOCATION = addr.DestLocation;
            header.DUE_DATE = DateTime.Parse(dueDate);
            header.ORDER_TYPE = orderType;//get form arg
            header.CUSTOMER_PO_NO = customerPONO;//--get form arg?
            header.SPL_IND = "Y";//?
            header.SHIPCOMP_FLAG = shipCompFlag;//Get from arg
            header.ORDER_STATUS = OrderStatusEnum.D.ToString();//draft ind
            header.CREATION_DATE = DateTime.Now;
            header.CREATED_BY = appUser;//appuser get from local arg
            header.RECORD_DATE = DateTime.Now;//?
            header.UPDATED_BY = appUser;
            header.ORDER_NOTES = orderNote;//Get from arg

            return header;
        }

    }
}
