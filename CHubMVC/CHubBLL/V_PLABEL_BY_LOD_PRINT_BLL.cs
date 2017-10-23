using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class V_PLABEL_BY_LOD_PRINT_BLL
    {
        private readonly V_PLABEL_BY_LOD_PRINT_DAL dal;

        public V_PLABEL_BY_LOD_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_LOD_PRINT_DAL();
        }

        public V_PLABEL_BY_LOD_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BY_LOD_PRINT_DAL(db);
        }

        public List<V_PLABEL_BY_LOD_PRINT> QueryBySearch(string LabelTYPE, string ShipmentNo, string BoxNumber, string PartNumber_RP, string PartNumber_GOMS)
        {
            return dal.QueryBySearch(LabelTYPE, ShipmentNo, BoxNumber, PartNumber_RP, PartNumber_GOMS);
        }


        public List<V_PLABEL_BY_LOD_PRINT> BatchGetLabelPrintData(List<string> VID, string LabelTYPE, string ShipmentNo, string BoxNumber, string PartNumber_RP, string PartNumber_GOMS)
        {
            return dal.BatchGetLabelPrintData(VID,  LabelTYPE,  ShipmentNo,  BoxNumber,  PartNumber_RP,  PartNumber_GOMS);
        }


        public string GetADRNAMEByShip_ID(string Ship_ID)
        {
            return dal.GetADRNAMEByShip_ID(Ship_ID);
        }

        public string GetADRNAMEByLODNUM(string LODNUM)
        {
            return dal.GetADRNAMEByLODNUM(LODNUM);
        }

    }
}
