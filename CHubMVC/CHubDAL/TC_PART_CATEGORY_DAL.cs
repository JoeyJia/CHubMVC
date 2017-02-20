using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class TC_PART_CATEGORY_DAL : BaseDAL
    {
        public TC_PART_CATEGORY_DAL()
            : base() { }

        public TC_PART_CATEGORY_DAL(CHubEntities db)
            : base(db) { }

        public List<TC_PART_CATEGORY> GetTCPartCategory()
        {
            return db.TC_PART_CATEGORY.ToList();
        }

    }
}
