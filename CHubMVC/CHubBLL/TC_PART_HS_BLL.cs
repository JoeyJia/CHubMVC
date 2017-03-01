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
    public class TC_PART_HS_BLL
    {
        private readonly TC_PART_HS_DAL dal;

        public TC_PART_HS_BLL()
        {
            dal = new TC_PART_HS_DAL();
        }
        public TC_PART_HS_BLL(CHubEntities db)
        {
            dal = new TC_PART_HS_DAL(db);
        }

        public bool Exist(string partNo)
        {
            return dal.Exist(partNo);
        }

        public TC_PART_HS GetTCPartHS(string partNo)
        {
            return dal.GetTCPartHS(partNo);
        }

        public void update(TC_PART_HS model,bool autoSave=true)
        {
            dal.Update(model,autoSave);
        }

        public void Add(TC_PART_HS model,bool autoSave = true)
        {
            dal.Add(model,autoSave);
        }

    }
}
