using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class M_PART_BLL
    {
        private readonly M_PART_DAL dal;

        public M_PART_BLL()
        {
            dal = new M_PART_DAL();
        }
        public M_PART_BLL(CHubEntities db)
        {
            dal = new M_PART_DAL(db);
        }

        public bool Exist(string partNo)
        {
            return dal.Exist(partNo);
        }

        public M_PART GetMPartByPartNo(string partNo)
        {
            return dal.GetMPartByPartNo(partNo);
        }

        public void Update(M_PART model,bool autoSave = true)
        {
            dal.Update(model,autoSave);
        }

    }
}
