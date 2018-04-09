using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class TC_PART_CATEGORY_BLL
    {
        private readonly TC_PART_CATEGORY_DAL dal;

        public TC_PART_CATEGORY_BLL()
        {
            dal = new TC_PART_CATEGORY_DAL();
        }
        public TC_PART_CATEGORY_BLL(CHubEntities db)
        {
            dal = new TC_PART_CATEGORY_DAL(db);
        }

        public List<TC_PART_CATEGORY> GetTCPartCategory()
        {
            return dal.GetTCPartCategory();
        }

        public List<string> GetTCGroupList()
        {
            return dal.GetTCGroupList();
        }

        public List<string> GetCIDList()
        {
            return dal.GetCIDList();
        }


    }
}
