using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.WebArg;

namespace CHubBLL
{
  public  class V_PLABEL_BY_KITS_PRINT_BLL
    {
        private readonly V_PLABEL_BY_KITS_PRINT_DAL dal;

        public V_PLABEL_BY_KITS_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_KITS_PRINT_DAL();
        }

        public V_PLABEL_BY_KITS_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BY_KITS_PRINT_DAL(db);
        }

        public List<V_PLABEL_BY_KITS_PRINT> SearchByKITS(string Label_Code, string Part_No)
        {
            return dal.SearchByKITS(Label_Code, Part_No);
        }

        public string GetLabelCodeDesc(string Label_Code)
        {
            return dal.GetLabelCodeDesc(Label_Code);
        }

        public string GetPartNoDescription(string Part_No)
        {
            return dal.GetPartNoDescription(Part_No);
        }

        public List<V_PLABEL_BY_KITS_PRINT> GetPrintDataByKITS( string LABEL_CODE, string PART_NO)
        {
            return dal.GetPrintDataByKITS( LABEL_CODE, PART_NO);
        }

        public int IsExistMore(GKITSArg arg)
        {
            return dal.IsExistMore(arg);
        }


        public List<V_PLABEL_BY_KITS_PRINT> GetKITSByVID(string VID)
        {
            return dal.GetKITSByVID(VID);
        }


    }
}
