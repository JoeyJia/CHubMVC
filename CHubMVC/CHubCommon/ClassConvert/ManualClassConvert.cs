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

        public static TS_OR_HEADER_STAGE ConvertExAliaAddr2HeaderStage(ExVAliasAddr addr, string seq, string dueDate,string orderType,string shipCompFlag,string customerPONO, string orderNote,bool isSpecialShip, string appUser,bool IsAltAddr = false)
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
            hStage.ORDER_TYPE = orderType;
            hStage.CUSTOMER_PO_NO = customerPONO;
            hStage.SPL_IND = ValueConvert.BoolToNY(isSpecialShip);
            hStage.SHIPCOMP_FLAG = shipCompFlag;
            hStage.ORDER_STATUS = OrderStatusEnum.D.ToString();//draft ind
            hStage.CREATION_DATE = DateTime.Now;
            hStage.CREATED_BY = appUser;//appuser get from local arg
            hStage.RECORD_DATE = DateTime.Now;
            hStage.UPDATED_BY = appUser;
            hStage.ORDER_NOTES = orderNote;

            return hStage;
        }

        public static TS_OR_HEADER ConvertExAliaAddr2Header(ExVAliasAddr addr,string seq, string dueDate, string orderType, string shipCompFlag, string customerPONO, string orderNote,bool isSpecialShip, string appUser, bool IsAltAddr = false)
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
            header.ORDER_TYPE = orderType;
            header.CUSTOMER_PO_NO = customerPONO;
            header.SPL_IND = ValueConvert.BoolToNY(isSpecialShip);
            header.SHIPCOMP_FLAG = shipCompFlag;
            header.ORDER_STATUS = OrderStatusEnum.S.ToString();//draft ind
            header.CREATION_DATE = DateTime.Now;
            header.CREATED_BY = appUser;//appuser get from local arg
            header.RECORD_DATE = DateTime.Now;
            header.UPDATED_BY = appUser;
            header.ORDER_NOTES = orderNote;

            return header;
        }

        public static TS_OR_DETAIL ConvertOLItem2Detail(OrderLineItem item, string seq, string appUser)
        {

            TS_OR_DETAIL detail = new TS_OR_DETAIL();
            detail.ORDER_REQ_NO = string.IsNullOrEmpty(seq) ? 0 : decimal.Parse(seq); ;
            detail.ORDER_LINE_NO = item.OrderLineNo;
            if (item.PriAVLCheck < item.Qty && item.AltAVLCheck >= item.Qty)
                detail.SHIPFROM_SEQ = 1;
            else
                detail.SHIPFROM_SEQ = 0;
            detail.PART_NO = item.PartNo;
            detail.CUSTOMER_PART_NO = item.CustomerPartNo;
            detail.BUY_QTY = item.Qty;
            detail.DESCRIPTION = item.Description;
            detail.DESC_CN = item.DescCN;
            detail.CREATION_DATE = DateTime.Now;
            detail.CREATED_BY = appUser;
            detail.UPDATED_DATE = null;
            detail.UPDATED_BY = null;
            return detail;
        }

        public static TS_OR_DETAIL_STAGE ConvertOLItem2DetailStage(OrderLineItem item,string seq,string appUser)
        {
            TS_OR_DETAIL_STAGE dStage = new TS_OR_DETAIL_STAGE();
            dStage.ORDER_REQ_NO = string.IsNullOrEmpty(seq) ? 0 : decimal.Parse(seq); ;
            dStage.ORDER_LINE_NO = item.OrderLineNo;
            if (item.PriAVLCheck < item.Qty && item.AltAVLCheck >= item.Qty)
                dStage.SHIPFROM_SEQ = 1;
            else
                dStage.SHIPFROM_SEQ = 0;
            dStage.PART_NO = item.PartNo;
            dStage.CUSTOMER_PART_NO = item.CustomerPartNo;
            dStage.BUY_QTY = item.Qty;
            dStage.DESCRIPTION = item.Description;
            dStage.DESC_CN = item.DescCN;
            dStage.DESCRIPTION = null;
            dStage.CREATION_DATE = DateTime.Now;
            dStage.CREATED_BY = appUser;
            dStage.UPDATED_DATE = null;
            dStage.UPDATED_BY = null;
            return dStage;
        }

    }
}
