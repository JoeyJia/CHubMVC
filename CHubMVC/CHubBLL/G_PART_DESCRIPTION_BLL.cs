using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using static CHubCommon.CHubEnum;

namespace CHubBLL
{
    public class G_PART_DESCRIPTION_BLL
    {
        private readonly G_PART_DESCRIPTION_DAL dal;

        public G_PART_DESCRIPTION_BLL()
        {
            dal = new G_PART_DESCRIPTION_DAL();
        }
        public G_PART_DESCRIPTION_BLL(CHubEntities db)
        {
            dal = new G_PART_DESCRIPTION_DAL(db);
        }

        public bool IsPartNoExist(string partNo)
        {
            return dal.IsPartNoExist(partNo);
        }

        public G_PART_DESCRIPTION GetPartDescription(string partNo)
        {
            return dal.GetPartDescription(partNo);
        }

        public List<G_PART_DESCRIPTION> fuzzyqueryByPartNo(string fuzzyPartNo)
        {
            return dal.fuzzyqueryByPartNo(fuzzyPartNo);
        }

        public bool IsInActive(string partNo)
        {
            return dal.IsInActive(partNo);
        }

    }
}
