using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

namespace CHubBLL
{
    public class V_XCEC_ADDR_ALL_BLL
    {
        private V_XCEC_ADDR_ALL_DAL dal;

        public V_XCEC_ADDR_ALL_BLL()
        {
            dal = new V_XCEC_ADDR_ALL_DAL();
        }

        public V_XCEC_ADDR_ALL_BLL(CHubEntities db)
        {
            dal = new V_XCEC_ADDR_ALL_DAL(db);
        }

        public List<V_XCEC_ADDR_ALL> SearchXcecAddrAll(string WAREHOUSE, string ADDR_NAME, string ADDR_1, string ADDR_2, string ADDR_3)
        {
            return dal.SearchXcecAddrAll(WAREHOUSE, ADDR_NAME, ADDR_1, ADDR_2, ADDR_3);
        }

        public bool SecureCheck(string User)
        {
            return dal.SecureCheck(User);
        }

        public List<V_XCEC_ADDR_ALL> GetXcecAddrAll(string WAREHOUSE, string DEST_LOCATION)
        {
            return dal.GetXcecAddrAll(WAREHOUSE, DEST_LOCATION);
        }

        public void UpdateXcecAddrAll(V_XCEC_ADDR_ALL addr, string XCEC_ADDR_SEQ, string User)
        {
            dal.UpdateXcecAddrAll(addr, XCEC_ADDR_SEQ, User);
        }
    }
}
