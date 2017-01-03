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
            var model = db.G_PART_DESCRIPTION.FirstOrDefault(a => a.PART_NO == partNo);
            if (model == null)
                return false;
            return true;
        }

    }
}
