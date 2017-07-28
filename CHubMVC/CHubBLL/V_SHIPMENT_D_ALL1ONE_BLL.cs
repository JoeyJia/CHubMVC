using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class V_SHIPMENT_D_ALL1ONE_BLL
    {
        private readonly V_SHIPMENT_D_ALL1ONE_DAL dal;

        public V_SHIPMENT_D_ALL1ONE_BLL()
        {
            dal = new V_SHIPMENT_D_ALL1ONE_DAL();
        }
        public V_SHIPMENT_D_ALL1ONE_BLL(CHubEntities db)
        {
            dal = new V_SHIPMENT_D_ALL1ONE_DAL(db);
        }

        public List<PLabelRow> GetPLabelRows(string shipmentNo, string boxNo, string printPartNo)
        {
            return dal.GetPLabelRows(shipmentNo,boxNo,printPartNo);
        }


    }
}
