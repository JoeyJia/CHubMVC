using CHubBLL;
using CHubCommon;
using CHubDBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHubMVC.Validations
{
    public class G_OESALES_CATALOG_VALIDATION
    {
        private string SysID;
        private string PartNo;
        private decimal Qty;
        public G_OESALES_CATALOG model;

        public G_OESALES_CATALOG_VALIDATION(string sysID, string partNo,decimal qty)
        {
            this.SysID = sysID;
            this.PartNo = partNo;
            this.Qty = qty;
        }

        public string ValidationAction()
        {
            if (string.IsNullOrEmpty(SysID) || string.IsNullOrEmpty(PartNo))
                throw new Exception("not init class right, empty args");

            string msg = string.Empty;
            G_OESALES_CATALOG_BLL bll = new G_OESALES_CATALOG_BLL();
            model = bll.GetOESalesCatalog(SysID, PartNo);

            if (model.MOQ > 1)
            {
                //if qty can be divide exactly by moq, skip warning
                if (Qty != 0 && Qty % model.MOQ != 0)
                    msg += string.Format("MOQ:{0},", model.MOQ);
            }

            if (model.MANUAL_ALLOCATED_FLAG == CHubConstValues.IndY)
                msg += "Manual Allocated,";

            return msg;
        }


    }
}