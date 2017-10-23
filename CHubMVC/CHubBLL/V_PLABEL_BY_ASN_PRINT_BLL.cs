using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDAL;

namespace CHubBLL
{
   public class V_PLABEL_BY_ASN_PRINT_BLL
    {
        private readonly V_PLABEL_BY_ASN_PRINT_DAL dal;

        public V_PLABEL_BY_ASN_PRINT_BLL()
        {
            dal = new V_PLABEL_BY_ASN_PRINT_DAL();
        }
        public V_PLABEL_BY_ASN_PRINT_BLL(CHubEntities db)
        {
            dal = new V_PLABEL_BY_ASN_PRINT_DAL(db);
        }

        public List<V_PLABEL_BY_ASN_PRINT> GetSearchByASN(string LABEL_CODE, string ASN_NO, string PRINT_PART_NO, string PART_NO)
        {
            return dal.GetSearchByASN(LABEL_CODE,ASN_NO,PRINT_PART_NO,PART_NO);
        }

        public List<V_PLABEL_BY_ASN_PRINT> GetPrintDatasByASN(List<string> VID, string LabelTYPE, string ASN_NO, string PART_NO_ASN, string PRINT_PART_NO_ASN)
        {
            return dal.GetPrintDatasByASN(VID, LabelTYPE, ASN_NO, PART_NO_ASN, PRINT_PART_NO_ASN);
        }

        public string GetCOMPANY_NAMEByASN_NO(string ASN_NO)
        {
            return dal.GetCOMPANY_NAMEByASN_NO(ASN_NO);
        }


    }
}
