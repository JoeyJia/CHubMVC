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
   public class G_KITS_BLL
    {
        private readonly G_KITS_DAL dal;

        public G_KITS_BLL()
        {
            dal = new G_KITS_DAL();
        }

        public G_KITS_BLL(CHubEntities db)
        {
            dal = new G_KITS_DAL(db);
        }


        public List<G_KITS> GetGKITSBySearch(string PARENT_PART, string COMPONENT_PART, long LINE_ITEM_NO)
        {
            return dal.GetGKITSBySearch(PARENT_PART,COMPONENT_PART,LINE_ITEM_NO); 
        }

        public void AddKITS(G_KITS gkits)
        {
            dal.AddKITS(gkits);
        }

        public void UpdateKITS(G_KITS old, G_KITS gkits)
        {
            dal.UpdateKITS(old, gkits);
        }

    }
}
