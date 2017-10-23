using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
  public  class V_PLABEL_BY_PART_PRINT_BLL
    {
        private readonly V_PLABEL_BY_PART_PRINT_DAL dal;

        public V_PLABEL_BY_PART_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_PART_PRINT_DAL();
        }

        public V_PLABEL_BY_PART_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BY_PART_PRINT_DAL(db);
        }

        public List<V_PLABEL_BY_PART_PRINT> GetSearchByParts(string Label_TYPE, string PRTNUM, string Part_No)
        {
            return dal.GetSearchByParts(Label_TYPE, PRTNUM, Part_No);
        }

        public List<V_PLABEL_BY_PART_PRINT> GetPrintDataByPart(List<string> partNoList, string LabelTYPE)
        {
            return dal.GetPrintDataByPart(partNoList, LabelTYPE);
        }



    }
}
