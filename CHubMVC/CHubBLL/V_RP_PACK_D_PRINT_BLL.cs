using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class V_RP_PACK_D_PRINT_BLL
    {
        private readonly V_RP_PACK_D_PRINT_DAL dal;

        public V_RP_PACK_D_PRINT_BLL()
        {
            dal = new V_RP_PACK_D_PRINT_DAL();
        }
        public V_RP_PACK_D_PRINT_BLL(CHubEntities db)
        {
            dal = new V_RP_PACK_D_PRINT_DAL(db);
        }

        public List<V_RP_PACK_D_PRINT> GetPackDetails(string lodNum)
        {
            return dal.GetPackDetails(lodNum);
        }

    }
}
