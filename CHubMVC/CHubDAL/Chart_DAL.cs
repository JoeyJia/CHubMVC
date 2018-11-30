using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

namespace CHubDAL
{
    public class Chart_DAL
    {
        private CHubCommonHelper ccHelper;
        public Chart_DAL()
        {
            ccHelper = new CHubCommonHelper();
        }

        public List<V_DASH_TMS01> GetTMSData()
        {
            string sql = string.Format(@"select A.SHIP_FROM,A.ADRCTY,A.GPS_LOCATION,sum(SHIPMENTS) as SHIPMENTS from 
                                            (select * from V_DASH_TMS01 where GPS_LOCATION is not null) A
                                        group by A.SHIP_FROM,A.ADRCTY,A.GPS_LOCATION order by SHIP_FROM");

            var result = ccHelper.Search<V_DASH_TMS01>(sql);
            return result;
        }

        public List<V_DASH_SHIP11> GetV_DASH_SHIP11()
        {
            string sql = string.Format(@"select * from V_DASH_SHIP11 where SHIP_TO is not null and GPS_LOCATION is not null order by SHIP_FROM");
            var result = ccHelper.Search<V_DASH_SHIP11>(sql);
            return result;
        }

        public List<V_DASH_SHIP12> GetV_DASH_SHIP12()
        {
            string sql = string.Format(@"select * from V_DASH_SHIP12 order by PERIOD, SHIP_FROM");
            var result = ccHelper.Search<V_DASH_SHIP12>(sql);
            return result;
        }

        public List<V_DASH_SHIP21> GetV_DASH_SHIP21()
        {
            string sql = string.Format(@"select * from V_DASH_SHIP21 where SHIP_TO is not null and GPS_COUNTRY is not null");
            var resutl = ccHelper.Search<V_DASH_SHIP21>(sql);
            return resutl;
        }

        public List<V_DASH_SHIP22> GetV_DASH_SHIP22()
        {
            string sql = string.Format(@"select * from V_DASH_SHIP22 order by PERIOD");
            var result = ccHelper.Search<V_DASH_SHIP22>(sql);
            return result;
        }

    }
}
