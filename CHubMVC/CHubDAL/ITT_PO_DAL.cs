using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class ITT_PO_DAL : BaseDAL
    {
        public ITT_PO_DAL()
            : base() { }

        public ITT_PO_DAL(CHubEntities db)
            : base(db) { }


        public List<ITT_PO_LEVEL_1> GetLevel1Data(string partNo)
        {
            var result = (
                from p in db.ITT_PO
                where p.PART_NO==partNo
                select new ITT_PO_LEVEL_1
                {
                 BUY_FOR=p.BUY_FOR,
                 PUR_ORDER_ID = p.PUR_ORDER_ID,
                 WAREHOUSE = p.WAREHOUSE,
                 BUY_FROM_COMPANY = p.BUY_FROM_COMPANY,
                 COMPANY_NAME = p.COMPANY_NAME,
                 EC_PUR_ORDER_TYPE_CODE = p.EC_PUR_ORDER_TYPE_CODE,
                 BUYER_CODE = p.BUYER_CODE,
                 SUPPLIER_TYPE = p.SUPPLIER_TYPE,
                 PO_STATUS=p.PO_STATUS
                }
                ).Distinct().OrderByDescending(a=>a.PUR_ORDER_ID).ToList();
            return result;
        }

        public List<ITT_PO_LEVEL_2> GetLevel2Data(string partNo,string poNo)
        {
            var result = (
                from p in db.ITT_PO
                where p.PART_NO == partNo 
                && p.PUR_ORDER_ID==poNo
                
                select new ITT_PO_LEVEL_2
                {
                    PUR_ORDER_ID = p.PUR_ORDER_ID,
                    PUR_ORDER_LINE_NO = p.PUR_ORDER_LINE_NO,
                    PART_NO = p.PART_NO,
                    PUR_ORDER_DESC = p.PUR_ORDER_DESC,
                    UOM_CODE=p.UOM_CODE
                }
                ).Distinct().ToList();
            return result;
        }

        public List<ITT_PO_Release> GetPOReleaseData(string partNo, string poNo,long poLineNo)
        {
            var result = (
                from p in db.ITT_PO
                where p.PART_NO == partNo
                && p.PUR_ORDER_ID == poNo
                && p.PUR_ORDER_LINE_NO==poLineNo

                select new ITT_PO_Release
                {
                    PUR_RELEASE_NO=p.PUR_RELEASE_NO,
                    RELEASE_DATE = p.RELEASE_DATE,
                    RELEASE_QTY = p.RELEASE_QTY,
                    REMAINING_QTY = p.REMAINING_QTY,
                    REVISED_DUE_DATE = p.REVISED_DUE_DATE,
                    ETA_DATE = p.ETA_DATE,
                    RECEIVING_DATE = p.RECEIVING_DATE,
                    PO_STATUS = p.PO_STATUS
                }
                ).Distinct().ToList();
            return result;
        }

    }
}
