using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class G_PART_DESCRIPTION_DAL : BaseDAL
    {
        public G_PART_DESCRIPTION_DAL()
            : base() { }

        public G_PART_DESCRIPTION_DAL(CHubEntities db)
            : base(db) { }

        public bool IsPartNoExist(string partNo)
        {
            return db.G_PART_DESCRIPTION.Any(a => a.PART_NO == partNo);
        }

    }
}
