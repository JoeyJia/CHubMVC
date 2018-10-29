using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubDAL;
using CHubModel.WebArg;
using System.Data;

namespace CHubBLL
{
    public class APP_QUICK_SCREEN_BLL
    {
        private readonly APP_QUICK_SCREEN_DAL dal;
        public APP_QUICK_SCREEN_BLL()
        {
            dal = new APP_QUICK_SCREEN_DAL();
        }
        public APP_QUICK_SCREEN_BLL(CHubEntities db)
        {
            dal = new APP_QUICK_SCREEN_DAL(db);
        }

        public List<APP_QUICK_SCREEN> GetAPPQUICKSCREENList()
        {
            return dal.GetAPPQUICKSCREENList();
        }

        //public List<TableColumnListArg> GetTableColum(string TABLE_NAME)
        //{
        //    return dal.GetTableColum(TABLE_NAME);
        //}

        public DataTable GetTableColum(string TABLE_NAME)
        {
            return dal.GetTableColum(TABLE_NAME);
        }

        public DataTable GetTableDatas(string TABLE_NAME)
        {
            return dal.GetTableDatas(TABLE_NAME);
        }

        public List<string> GetQUICK_SCREEN()
        {
            return dal.GetQUICK_SCREEN();
        }

        public APP_QUICK_SCREEN GetTableName(string QUICK_SCREEN)
        {
            return dal.GetTableName(QUICK_SCREEN);
        }

        public string GetPRIMARY_KEY(string TABLE_NAME)
        {
            return dal.GetPRIMARY_KEY(TABLE_NAME);
        }

        public void UpdateOrInsertQuickScreen(string sql)
        {
            dal.UpdateOrInsertQuickScreen(sql);
        }

    }
}
