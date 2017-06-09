using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class RP_ADR_MST_DAL : BaseDAL
    {
        public RP_ADR_MST_DAL()
            : base() { }

        public RP_ADR_MST_DAL(CHubEntities db)
            : base(db) { }

        public List<ExRPADRMst> GetADRListByCustName(string custName)
        {
            var result = (
                from a in db.RP_ADR_MST
                join t in db.RP_CUST_PACK_TYPE on a.CUST_PACK_ID equals t.CUST_PACK_ID into temp
                from tt in temp.DefaultIfEmpty()
                where a.ADRNAM.Contains(custName)
                select new ExRPADRMst
                {
                    WH_ID = a.WH_ID,
                    ADRNAM = a.ADRNAM,
                    CUST_PACK_ID = a.CUST_PACK_ID,
                    PACK_DESC = tt==null?null:tt.PACK_DESC,
                    CUST_SHORT_NAME = tt == null ? null : tt.CUST_SHORT_NAME,
                    AUTO_PRINT = tt == null ? null : tt.AUTO_PRINT
                }
                );
            return result.ToList();
        }

        public void SaveCustPackID(ExRPADRMst model)
        {
            RP_ADR_MST exist = db.RP_ADR_MST.FirstOrDefault(a => a.WH_ID == model.WH_ID && a.ADRNAM == model.ADRNAM);
            exist.CUST_PACK_ID = model.CUST_PACK_ID;
            Update(exist);
        }

    }
}
