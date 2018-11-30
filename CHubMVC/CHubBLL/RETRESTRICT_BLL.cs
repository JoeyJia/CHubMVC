using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;
using CHubModel.WebArg;
using System.Data;

namespace CHubBLL
{
    public class RETRESTRICT_BLL
    {
        private RETRESTRICT_DAL dal;
        public RETRESTRICT_BLL()
        {
            dal = new RETRESTRICT_DAL();
        }

        public List<V_RET_PART_RESTRICT> RetRestrictSearch(string PART_NO)
        {
            return dal.RetRestrictSearch(PART_NO);
        }
        public void RetRestrictSave(RetRestrictArg arg)
        {
            dal.RetRestrictSave(arg);
        }
        public string GetSql()
        {
            return dal.GetSql();
        }
        public DataTable GetDataTableBySql(string sql)
        {
            return dal.GetDataTableBySql(sql);
        }
    }
}
