using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class IA_CODE_TYPE_DAL : BaseDAL
    {
        public IA_CODE_TYPE_DAL() : base()
        {

        }

        public IA_CODE_TYPE_DAL(CHubEntities db) : base(db)
        {

        }

        public List<IA_CODE_TYPE> GetIACodes()
        {
            string sql = string.Format(@"select * from IA_CODE_TYPE");
            var result = db.Database.SqlQuery<IA_CODE_TYPE>(sql);
            return result.ToList();
        }

        public IA_CODE_TYPE GetIACode(string IACode)
        {
            string sql = string.Format(@"select * from IA_CODE_TYPE where IA_CODE='{0}'", IACode);
            var result = db.Database.SqlQuery<IA_CODE_TYPE>(sql);
            return result.FirstOrDefault();
        }

        public bool IsExist(string IACode)
        {
            string sql = string.Format(@"select * from IA_CODE_TYPE where IA_CODE='{0}'", IACode);
            var result = db.Database.SqlQuery<IA_CODE_TYPE>(sql);
            if (result != null && result.Any())
                return true;
            else
                return false;
        }

        public void AddOrUpdateIACode(IA_CODE_TYPE iacode, string Type)
        {
            if (Type == "Update")//Update
                base.Update(iacode);
            else
                base.Add(iacode);
        }


        public List<CHubDBEntity.UnmanagedModel.IA_CODE_TYPE> GetIACODEList()
        {
            string sql = string.Format(@"select IA_CODE from IA_CODE_TYPE where auto_flag = 'N'");
            var result = db.Database.SqlQuery<CHubDBEntity.UnmanagedModel.IA_CODE_TYPE>(sql);
            return result.ToList();
        }


    }
}
