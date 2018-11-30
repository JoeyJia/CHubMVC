using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using Oracle.ManagedDataAccess.Client;

namespace CHubDAL
{
    public class DL_DAL
    {
        private CHubCommonHelper ccHelper;
        public DL_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<string> GetLOAD_TYPEs(string appUser)
        {
            List<string> str = new List<string>();
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE_AUTH where APP_USER='{0}' and AUTH_BEGIN_DATE <=sysdate and (AUTH_END_DATE>=sysdate or AUTH_END_DATE is null)", appUser);
            var result = ccHelper.Search<IHUB_LOAD_TYPE_AUTH>(sql);
            str = result.Select(a => a.LOAD_TYPE).ToList();
            return str;
        }

        public string GetLOAD_DESC(string LOAD_TYPE)
        {
            string str = string.Empty;
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE where LOAD_TYPE='{0}'", LOAD_TYPE);
            var result = ccHelper.Search<IHUB_LOAD_TYPE>(sql);
            str = result.Select(a => a.LOAD_DESC).FirstOrDefault();
            return str;
        }

        public IHUB_LOAD_TYPE GetIHUB_LOAD_TYPE(string LOAD_TYPE)
        {
            string sql = string.Format(@"select * from IHUB_LOAD_TYPE where LOAD_TYPE='{0}'", LOAD_TYPE);
            var result = ccHelper.Search<IHUB_LOAD_TYPE>(sql).FirstOrDefault();
            return result;
        }


        public string GetLOAD_BATCH()
        {
            string sql = string.Format(@"select SEQ_IHUB_LOAD.NEXTVAL from dual");
            var result = ccHelper.ExecuteFunc(sql);
            return result;
        }


        public void AddIHUB_LOAD_BASE(IHUB_LOAD_BASE ilb, string LOAD_BATCH, string LOAD_TYPE, string LOAD_BY, string LOAD_LINE_NO)
        {
            string sql = string.Format(@"insert into IHUB_LOAD_BASE(
                                LOAD_BATCH,LOAD_TYPE,LOAD_BY,LOAD_DATE,ERR_MSG,LOAD_LINE_NO,
                                T01,T02,T03,T04,T05,T06,T07,T08,T09,T10,T11,T12,
                                T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24) values(
                                '{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),'{4}','{5}',
                                '{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
                                '{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}'
                                )", LOAD_BATCH, LOAD_TYPE, LOAD_BY, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "", LOAD_LINE_NO,
                                ilb.T01,ilb.T02,ilb.T03,ilb.T04,ilb.T05,ilb.T06,ilb.T07,ilb.T08,ilb.T09,ilb.T10,ilb.T11,ilb.T12,
                                ilb.T13,ilb.T14,ilb.T15,ilb.T16,ilb.T17,ilb.T18,ilb.T19,ilb.T20,ilb.T21,ilb.T22,ilb.T23,ilb.T24
                                );
            ccHelper.Update(sql);



            //string sql = string.Format(@"insert into IHUB_LOAD_BASE(
            //                        LOAD_BATCH,LOAD_TYPE,LOAD_BY,LOAD_DATE,ERR_MSG,LOAD_LINE_NO,
            //                        T01,T02,T03,T04,T05,T06,T07,T08,T09,T10,T11,T12,
            //                        T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24) values (
            //                        :LOAD_BATCH,:LOAD_TYPE,:LOAD_BY,:LOAD_DATE,:ERR_MSG,:LOAD_LINE_NO,
            //                        :T01,:T02,:T03,:T04,:T05,:T06,:T07,:T08,:T09,:T10,:T11,:T12,
            //                        :T13,:T14,:T15,:T16,:T17,:T18,:T19,:T20,:T21,:T22,:T23,:T24)");
            //OracleParameter[] param = new OracleParameter[] {
            //    new OracleParameter(":LOAD_BATCH",OracleDbType.Decimal),
            //    new OracleParameter(":LOAD_TYPE",OracleDbType.Varchar2),
            //    new OracleParameter(":LOAD_BY",OracleDbType.Varchar2),
            //    new OracleParameter(":LOAD_DATE",OracleDbType.Date),
            //    new OracleParameter(":ERR_MSG",OracleDbType.Varchar2),
            //    new OracleParameter(":LOAD_LINE_NO",OracleDbType.Decimal),
            //    new OracleParameter(":T01",OracleDbType.Varchar2),
            //    new OracleParameter(":T02",OracleDbType.Varchar2),
            //    new OracleParameter(":T03",OracleDbType.Varchar2),
            //    new OracleParameter(":T04",OracleDbType.Varchar2),
            //    new OracleParameter(":T05",OracleDbType.Varchar2),
            //    new OracleParameter(":T06",OracleDbType.Varchar2),
            //    new OracleParameter(":T07",OracleDbType.Varchar2),
            //    new OracleParameter(":T08",OracleDbType.Varchar2),
            //    new OracleParameter(":T09",OracleDbType.Varchar2),
            //    new OracleParameter(":T10",OracleDbType.Varchar2),
            //    new OracleParameter(":T11",OracleDbType.Varchar2),
            //    new OracleParameter(":T12",OracleDbType.Varchar2),
            //    new OracleParameter(":T13",OracleDbType.Varchar2),
            //    new OracleParameter(":T14",OracleDbType.Varchar2),
            //    new OracleParameter(":T15",OracleDbType.Varchar2),
            //    new OracleParameter(":T16",OracleDbType.Varchar2),
            //    new OracleParameter(":T17",OracleDbType.Varchar2),
            //    new OracleParameter(":T18",OracleDbType.Varchar2),
            //    new OracleParameter(":T19",OracleDbType.Varchar2),
            //    new OracleParameter(":T20",OracleDbType.Varchar2),
            //    new OracleParameter(":T21",OracleDbType.Varchar2),
            //    new OracleParameter(":T22",OracleDbType.Varchar2),
            //    new OracleParameter(":T23",OracleDbType.Varchar2),
            //    new OracleParameter(":T24",OracleDbType.Varchar2)
            //};
            //param[0].Value = Convert.ToDecimal(LOAD_BATCH);
            //param[1].Value = LOAD_TYPE;
            //param[2].Value = LOAD_BY;
            //param[3].Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            //param[4].Value = "";
            //param[5].Value = Convert.ToDecimal(LOAD_LINE_NO);
            //param[6].Value = ilb.T01;
            //param[7].Value = ilb.T02;
            //param[8].Value = ilb.T03;
            //param[9].Value = ilb.T04;
            //param[10].Value = ilb.T05;
            //param[11].Value = ilb.T06;
            //param[12].Value = ilb.T07;
            //param[13].Value = ilb.T08;
            //param[14].Value = ilb.T09;
            //param[15].Value = ilb.T10;
            //param[16].Value = ilb.T11;
            //param[17].Value = ilb.T12;
            //param[18].Value = ilb.T13;
            //param[19].Value = ilb.T14;
            //param[20].Value = ilb.T15;
            //param[21].Value = ilb.T16;
            //param[22].Value = ilb.T17;
            //param[23].Value = ilb.T18;
            //param[24].Value = ilb.T19;
            //param[25].Value = ilb.T20;
            //param[26].Value = ilb.T21;
            //param[27].Value = ilb.T22;
            //param[28].Value = ilb.T23;
            //param[29].Value = ilb.T24;

            //ccHelper.AddOrUpdateWithParams(sql, param);
        }

        public void AddIHUB_LOAD_BASE(IHUB_LOAD_BASE ilb, string LOAD_BATCH, string LOAD_TYPE, string LOAD_BY, string LOAD_LINE_NO, string INPUT1, string INPUT2, string INPUT3)
        {
            string sql = string.Format(@"insert into IHUB_LOAD_BASE(
                                LOAD_BATCH,LOAD_TYPE,LOAD_BY,LOAD_DATE,ERR_MSG,LOAD_LINE_NO,
                                T01,T02,T03,T04,T05,T06,T07,T08,T09,T10,T11,T12,
                                T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,INPUT1,INPUT2,INPUT3) values(
                                '{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),'{4}','{5}',
                                '{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
                                '{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'
                                )", LOAD_BATCH, LOAD_TYPE, LOAD_BY, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "", LOAD_LINE_NO,
                                ilb.T01, ilb.T02, ilb.T03, ilb.T04, ilb.T05, ilb.T06, ilb.T07, ilb.T08, ilb.T09, ilb.T10, ilb.T11, ilb.T12,
                                ilb.T13, ilb.T14, ilb.T15, ilb.T16, ilb.T17, ilb.T18, ilb.T19, ilb.T20, ilb.T21, ilb.T22, ilb.T23, ilb.T24, INPUT1, INPUT2, INPUT3
                                );
            ccHelper.Update(sql);
        }


        public void ExecP_IHUB_LOAD_POST(decimal LOAD_BATCH, string LOAD_TYPE)
        {
            OracleParameter[] param = new OracleParameter[] {
                new OracleParameter("V_LOAD_batch",OracleDbType.Decimal),
                new OracleParameter("LOAD_TYPE",OracleDbType.Varchar2)
            };
            param[0].Value = LOAD_BATCH;
            param[1].Value = LOAD_TYPE;

            ccHelper.ExecProcedure("P_IHUB_lOAD_POST", param);
        }

    }
}
