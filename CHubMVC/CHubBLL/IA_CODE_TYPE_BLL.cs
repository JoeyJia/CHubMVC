using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDAL;

namespace CHubBLL
{
    public class IA_CODE_TYPE_BLL
    {
        private IA_CODE_TYPE_DAL dal;

        public IA_CODE_TYPE_BLL()
        {
            dal = new IA_CODE_TYPE_DAL();
        }

        public IA_CODE_TYPE_BLL(CHubEntities db)
        {
            dal = new IA_CODE_TYPE_DAL(db);
        }

        public List<IA_CODE_TYPE> GetIACodes()
        {
            return dal.GetIACodes();
        }

        public IA_CODE_TYPE GetIACode(string IACode)
        {
            return dal.GetIACode(IACode);
        }

        public bool IsExist(string IACode)
        {
            return dal.IsExist(IACode);
        }

        public void AddOrUpdateIACode(IA_CODE_TYPE iacode, string Type)
        {
            dal.AddOrUpdateIACode(iacode, Type);
        }

        public List<CHubDBEntity.UnmanagedModel.IA_CODE_TYPE> GetIACODEList()
        {
            return dal.GetIACODEList();
        }


    }
}
