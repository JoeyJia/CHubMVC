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
    public class G_PART_ADDTIONAL_BLL
    {
        private G_PART_ADDTIONAL_DAL dal;

        public G_PART_ADDTIONAL_BLL()
        {
            dal = new G_PART_ADDTIONAL_DAL();
        }

        public G_PART_ADDTIONAL_BLL(CHubEntities db)
        {
            dal = new G_PART_ADDTIONAL_DAL(db);
        }

        public List<RP_LABEL_PAPER_TYPE> GetPaperIDList()
        {
            return dal.GetPaperIDList();
        }

        public List<G_PART_ADDTIONAL> GetPAByPartNo(string Part_NO)
        {
            return dal.GetPAByPartNo(Part_NO);
        }

        public void SavePrtAddt(G_PART_ADDTIONAL gpa)
        {
            dal.SavePrtAddt(gpa);
        }

        public GPARTDESCArg GetDescByPartNo(string PART_No)
        {
            return dal.GetDescByPartNo(PART_No);
        }

        public GPARTDESCArg GetDescByPrintPartNo(string PRINT_PART_NO)
        {
            return dal.GetDescByPrintPartNo(PRINT_PART_NO);
        }

    }
}
