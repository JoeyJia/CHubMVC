using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class V_IA_PRT_NOTE_CUST_ADDNEW_BLL
    {
        private readonly V_IA_PRT_NOTE_CUST_ADDNEW_DAL dal;
        public V_IA_PRT_NOTE_CUST_ADDNEW_BLL()
        {
            dal = new V_IA_PRT_NOTE_CUST_ADDNEW_DAL();
        }

        public V_IA_PRT_NOTE_CUST_ADDNEW_BLL(CHubEntities db)
        {
            dal = new V_IA_PRT_NOTE_CUST_ADDNEW_DAL(db);
        }

        public List<V_IA_PRT_NOTE_CUST_ADDNEW> GetIANOTECNEW(string PART_NO, string ADRNAM)
        {
            return dal.GetIANOTECNEW(PART_NO, ADRNAM);
        }

        public void IANOTECSaveNew(string PART_NO, string ADRNAM, string QC_NOTE, string userID)
        {
            dal.IANOTECSaveNew(PART_NO, ADRNAM, QC_NOTE, userID);
        }
    }
}
