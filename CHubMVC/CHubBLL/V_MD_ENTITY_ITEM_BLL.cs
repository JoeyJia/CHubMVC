using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_MD_ENTITY_ITEM_BLL
    {
        private V_MD_ENTITY_ITEM_DAL dal;

        public V_MD_ENTITY_ITEM_BLL()
        {
            dal = new V_MD_ENTITY_ITEM_DAL();
        }

        public List<V_MD_ENTITY_ITEM> MDJvItemSearch(string Part_NO)
        {
            return dal.MDJvItemSearch(Part_NO);
        }

    }
}
