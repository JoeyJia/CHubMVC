using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;
using CHubCommon;

namespace CHubDAL
{
    public class TC_HSCODE_MST_DAL : BaseDAL
    {
        private CHubCommonHelper ccHelper;
        public TC_HSCODE_MST_DAL() : base()
        {
            ccHelper = new CHubCommonHelper();
        }

        public TC_HSCODE_MST_DAL(CHubEntities db) : base(db)
        {

        }


        public List<CHubDBEntity.UnmanagedModel.TC_HSCODE_MST> GetHSCODEByCode(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_MST where HSCODE like '%{0}%'", HSCODE);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<CHubDBEntity.UnmanagedModel.TC_HSCODE_MST>(sql);
            return result.ToList();
        }

        public bool IsExistHSCODE(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_MST where HSCODE='{0}'", HSCODE);
            var result = db.Database.SqlQuery<CHubDBEntity.UnmanagedModel.TC_HSCODE_MST>(sql).FirstOrDefault();
            //var result = db.TC_HSCODE_MST.AsNoTracking().FirstOrDefault(h => h.HSCODE == HSCODE);
            if (result != null)
                return true;
            else
                return false;
        }

        public void AddOrUpdate(CHubDBEntity.UnmanagedModel.TC_HSCODE_MST tc, string type)
        {
            this.CheckCultureInfoForDate();
            string sql = string.Empty;
            if (type == "Update")
            {
                sql = string.Format(@"update TC_HSCODE_MST 
                                    set HSCODE_DESC='{0}',
                                        TC_CATEGORY_ID='{1}',
                                        TAX_REFUND_RATE='{2}',
                                        MFN_RATE='{3}',
                                        UOM='{4}',
                                        REGULATION='{5}',
                                        NOTE1='{6}',NOTE2='{7}',NOTE3='{8}',
                                        RECORD_DATE={9},UPDATED_BY='{10}'
                                    where HSCODE='{11}'",
                                    tc.HSCODE_DESC, tc.TC_CATEGORY_ID, tc.TAX_REFUND_RATE == 0 ? null : tc.TAX_REFUND_RATE, tc.MFN_RATE == 0 ? null : tc.MFN_RATE, tc.UOM,tc.REGULATION, tc.NOTE1, tc.NOTE2, tc.NOTE3, "sysdate", tc.UPDATED_BY, tc.HSCODE);
                //var ts = db.TC_HSCODE_MST.AsNoTracking().FirstOrDefault(h => h.HSCODE == tc.HSCODE);
                //ts.HSCODE_DESC = tc.HSCODE_DESC;
                //ts.TC_CATEGORY_ID = tc.TC_CATEGORY_ID;
                //ts.NOTE1 = tc.NOTE1;
                //ts.NOTE2 = tc.NOTE2;
                //ts.NOTE3 = tc.NOTE3;
                //ts.RECORD_DATE = tc.RECORD_DATE;
                //ts.TAX_REFUND_RATE = tc.TAX_REFUND_RATE == 0 ? null : tc.TAX_REFUND_RATE;
                //base.Update(ts);
            }
            else
                sql = string.Format(@"insert into TC_HSCODE_MST(
                                    HSCODE,
                                    HSCODE_DESC,
                                    TC_CATEGORY_ID,
                                    NOTE1,
                                    NOTE2,
                                    NOTE3,
                                    CREATE_DATE,
                                    TAX_REFUND_RATE,
                                    MFN_RATE,
                                    UOM,
                                    REGULATION)
                                    values('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}')",
                                    tc.HSCODE, tc.HSCODE_DESC, tc.TC_CATEGORY_ID, tc.NOTE1, tc.NOTE2, tc.NOTE3, "sysdate", tc.TAX_REFUND_RATE, tc.MFN_RATE, tc.UOM, tc.REGULATION);
            ccHelper.Update(sql);
        }


        public List<TC_HSCODE_AUDIT> GetHsCodeAudit(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_AUDIT where HSCODE ='{0}' order by ACTIVITY_DATE desc", HSCODE);
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<TC_HSCODE_AUDIT>(sql);
            return result.ToList();
        }



    }
}
