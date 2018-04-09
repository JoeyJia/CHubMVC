using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class IA_PRT_NOTE_CUST_BLL
    {
        private readonly IA_PRT_NOTE_CUST_DAL dal;
        public IA_PRT_NOTE_CUST_BLL()
        {
            dal = new IA_PRT_NOTE_CUST_DAL();
        }
        public IA_PRT_NOTE_CUST_BLL(CHubEntities db)
        {
            dal = new IA_PRT_NOTE_CUST_DAL(db);
        }

        public List<IA_PRT_NOTE_CUST> GetIANOTEC(string PART_NO, string PRINT_PART_NO)
        {
            return dal.GetIANOTEC(PART_NO, PRINT_PART_NO);
        }

        public IA_PRT_NOTE_CUST GetIANoteC(string PART_NO, string ADRNAM)
        {
            return dal.GetIANoteC(PART_NO, ADRNAM);
        }

        public void SaveIANOTEC(IA_PRT_NOTE_CUST ianotec)
        {
            dal.SaveIANOTEC(ianotec);
        }

    }
}
