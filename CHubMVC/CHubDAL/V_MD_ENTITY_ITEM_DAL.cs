using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class V_MD_ENTITY_ITEM_DAL
    {
        private CHubCommonHelper cchelper;
        public V_MD_ENTITY_ITEM_DAL()
        {
            cchelper = new CHubCommonHelper();
        }

        public List<V_MD_ENTITY_ITEM> MDJvItemSearch(string Part_NO)
        {
            string sql = string.Format(@"select * from V_MD_ENTITY_ITEM where PART_NO like '%{0}%'", Part_NO);
            var result = cchelper.ExecuteSqlToList<V_MD_ENTITY_ITEM>(sql);
            return result;
        }
    }
}
