using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
   public class RP_LABEL_TYPE2_BLL
    {
        private readonly RP_LABEL_TYPE2_DAL dal;

        public RP_LABEL_TYPE2_BLL()
        {
            dal = new RP_LABEL_TYPE2_DAL();
        }
        public RP_LABEL_TYPE2_BLL(CHubEntities db)
        {
            dal = new RP_LABEL_TYPE2_DAL(db);
        }



        public List<string> GetLABEL_CODEList()
        {
            return dal.GetLABEL_CODEList();
        }


        public string GetLabel_DESC(string Label_TYPE)
        {
            return dal.GetLabel_DESC(Label_TYPE);
        }
        

    }
}
