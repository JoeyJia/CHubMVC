using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class M_INV_DAL : BaseDAL
    {
        public M_INV_DAL()
            : base() { }

        public M_INV_DAL(CHubEntities db)
            : base(db) { }

        public List<M_INV> GetInterPDCData(string partNo)
        {
            return db.M_INV.Where(a => a.PART_NO == partNo).ToList();
        }

    }
}
