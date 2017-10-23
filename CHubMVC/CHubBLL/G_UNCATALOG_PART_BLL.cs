using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
   public class G_UNCATALOG_PART_BLL
    {
        private readonly G_UNCATALOG_PART_DAL dal;

        public G_UNCATALOG_PART_BLL()
        {
            dal = new G_UNCATALOG_PART_DAL();
        }

        public G_UNCATALOG_PART_BLL(CHubEntities db)
        {
            dal = new G_UNCATALOG_PART_DAL(db);
        }

        public List<G_UNCATALOG_PART> GetUncatalogPart(string PART_NO, string PRINT_PART_NO)
        {
            return dal.GetUncatalogPart(PART_NO, PRINT_PART_NO);
        }

        public List<G_UNCATALOG_PART> GetUncatalogByPART_NO(string PART_NO)
        {
            return dal.GetUncatalogByPART_NO(PART_NO);
        }


        public void AddUncatalogPart(G_UNCATALOG_PART gp)
        {
            dal.AddUncatalogPart(gp);
        }

        public void ModifyUncatalogPart(G_UNCATALOG_PART gp)
        {
            dal.ModifyUncatalogPart(gp);
        }

    }
}
