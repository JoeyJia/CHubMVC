using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
   public class V_PLABEL_BY_UNCATALOG_PRINT_BLL
    {
        private readonly V_PLABEL_BY_UNCATALOG_PRINT_DAL dal;

        public V_PLABEL_BY_UNCATALOG_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_UNCATALOG_PRINT_DAL();
        }

        public V_PLABEL_BY_UNCATALOG_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BY_UNCATALOG_PRINT_DAL(db);
        }

        public List<V_PLABEL_BY_UNCATALOG_PRINT> GetSearchByUncatalog(string Label_TYPE, string PRINT_PART_NO_UParts, string PART_NO_UParts)
        {
            return dal.GetSearchByUncatalog(Label_TYPE, PRINT_PART_NO_UParts, PART_NO_UParts);
        }

        public List<V_PLABEL_BY_UNCATALOG_PRINT> GetPrintDataByUncatalog(List<string> partNoList, string LabelTYPE, string PRINT_PART_NO_UParts, string PART_NO_UParts)
        {
            return dal.GetPrintDataByUncatalog(partNoList, LabelTYPE, PRINT_PART_NO_UParts, PART_NO_UParts);
        }



    }
}
