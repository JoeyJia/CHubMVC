using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class MP_ADDRMAP_BLL
    {
        private MP_ADDRMAP_DAL dal;
        public MP_ADDRMAP_BLL()
        {
            dal = new MP_ADDRMAP_DAL();
        }

        public List<V_E_ADDR_MST> GetV_E_ADDR_MST(V_E_ADDR_MST condition)
        {
            return dal.GetV_E_ADDR_MST(condition);
        }
        public List<V_E_ADDR_MST> GetCustomerAddress(string ADDR_TOKEN, string TO_SYSTEM, string ABBR, string DEST_LOCATION)
        {
            return dal.GetCustomerAddress(ADDR_TOKEN, TO_SYSTEM, ABBR, Convert.ToDecimal(DEST_LOCATION));
        }
        public List<V_E_ADDR_MST> GetCustomerAddress(string ADDR_TOKEN)
        {
            return dal.GetCustomerAddress(ADDR_TOKEN);
        }
        public List<G_ADDR_SPL> GetGomsAddress(string TO_SYSTEM, string ABBR, string DEST_LOCATION)
        {
            return dal.GetGomsAddress(TO_SYSTEM, ABBR, DEST_LOCATION);
        }
        public List<G_ADDR_SPL> GetGomsAddressByKeyWord(string keyWord, string TO_SYSTEM, string ABBR)
        {
            return dal.GetGomsAddressByKeyWord(keyWord,TO_SYSTEM,ABBR);
        }
        public void UpdateE_ADDR_MST(string ADDR_TOKEN, string DEST_LOCATION, string APP_USER)
        {
            dal.UpdateE_ADDR_MST(ADDR_TOKEN, DEST_LOCATION, APP_USER);
        }
        public List<G_ADDR_SPL> RefreshGomsAddress(string ADDR_TOKEN, string SYSID, string ABBREVIATION, string DEST_LOCATION, string APP_USER)
        {
            return dal.RefreshGomsAddress(ADDR_TOKEN, SYSID, ABBREVIATION, DEST_LOCATION, APP_USER);
        }
    }
}
